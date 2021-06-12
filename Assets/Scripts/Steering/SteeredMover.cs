using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SteeredMover : MonoBehaviour
{
	enum PlaneComponent
	{
		All,
		Plane,
		Normal
	}

	// TODO Rigidbody with collisions might get expensive over many instances. Is RigidBody usefully faster
	private Rigidbody body = null;
	public Rigidbody Body => Body;

	private Vector3 up => Vector3.forward;
	private Vector3 forwardAxis => Vector3.Cross(up, Vector3.right);
	private Vector3 sideAxis => Vector3.Cross(forwardAxis, up);

	[SerializeField] private MoveStats defaultStats;
	private MoveStats stats;
	private bool moving;

	private Vector3 externalForce = Vector3.zero;

	List<SteeringBehavior> steeringBehaviors = new List<SteeringBehavior>();
	List<float> modifiedWeights = new List<float>();
	List<Vector3> cachedDestinations = new List<Vector3>();

	[SerializeField] private float maxDestinationDistance = 100;

	// TODO This might be expensive for many instances. Maybe just register listeners without events.
	public UnityEvent OnMoveBegin = null;
	public UnityEvent OnMoveEnd = null;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		stats = defaultStats;
	}

	public void RegisterSteering(SteeringBehavior steering)
	{
		steeringBehaviors.Add(steering);
		modifiedWeights.Add(0);
		cachedDestinations.Add(Vector3.zero);
	}

	public void UnregisterSteering(SteeringBehavior steering)
	{
		steeringBehaviors.Remove(steering);
		modifiedWeights.RemoveAt(modifiedWeights.Count -1);
		cachedDestinations.RemoveAt(cachedDestinations.Count -1);
	}

	private void FixedUpdate()
	{
		/*var laser = Laser.Instance;
		var laserPos = laser.transform.position;
		var toLaser = laserPos - transform.position;
		bool chase = laser.Activated && toLaser.sqrMagnitude > Helper.Epsilon;*/

		// Combine steering behaviors to compute destination relative to current position.
		Vector3 destination = Vector3.zero;
		float weightSum = 0;
		float maxSqrDist = maxDestinationDistance * maxDestinationDistance;
		for (int i = steeringBehaviors.Count - 1; i >= 0; i--)
		{
			var steering = steeringBehaviors[i];
			if (steering == null)
			{
				steeringBehaviors.RemoveAt(i);
				modifiedWeights.RemoveAt(modifiedWeights.Count - 1);
				cachedDestinations.RemoveAt(cachedDestinations.Count - 1);
			}
			else
			{
				Vector3 steeringRequest = steering.ComputeDestinationRelative();
				var modifiedWeight = steering.Weight * (1 - Mathf.Min(steeringRequest.sqrMagnitude / maxSqrDist, 1));
				weightSum += modifiedWeight;
				modifiedWeights[i] = modifiedWeight;
				cachedDestinations[i] = steeringRequest;
			}
		}

		// Prevent weight from scaling force up. If all the weights are small then steering can be minimal.
		if (weightSum < 1)
		{
			weightSum = 1;
		}

		for (int i = steeringBehaviors.Count - 1; i >= 0; i--)
		{
			var steeringRequest = cachedDestinations[i];
			steeringRequest *= (modifiedWeights[i] / weightSum);
			destination += steeringRequest;
		}


		var moveForward = ConstrainMove(destination);
		bool attemptingForward = moveForward.sqrMagnitude > Helper.Epsilon;

		if (attemptingForward)
		{
			if (!moving)
			{
				moving = true;
				OnMoveBegin.Invoke();
			}
		}
		else
		{
			if (moving)
			{
				moving = false;
				OnMoveEnd.Invoke();
			}
		}

		if (destination.sqrMagnitude > Helper.Epsilon)
		{
			var lookAt = destination.normalized;

			// Arc Cosine is only define in [-1, 1] so prevent Dot Product from rounding error past that.
			var cos = Mathf.Min(Vector3.Dot(transform.up, lookAt), 1);
			var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;

			if (Vector3.Dot(transform.right, lookAt) > 0)
			{
				angle *= -1;
			}

			// If attempting to turn around completly, randomly choose a direction to turn.
			if (Mathf.Abs(Vector3.Dot(transform.up, lookAt) + 1) < Helper.Epsilon
			    && Random.Range(0f, 1f) < 0.5f)
			{
				angle = -180;
			}

			var maxTurn = stats.turnSpeed * Time.deltaTime;
			if (Mathf.Abs(angle) > maxTurn)
			{
				angle = angle > 0 ? maxTurn : -maxTurn;
			}

			angle += transform.rotation.eulerAngles.z;

			transform.rotation = Quaternion.AngleAxis(angle, up);

			if (attemptingForward)
			{
				body.AddForce(moveForward);
			}
		}

		if (body.velocity.sqrMagnitude > stats.maxSpeed * stats.maxSpeed)
		{
			body.velocity = body.velocity.normalized * stats.maxSpeed;
		}

		body.AddForce(externalForce, ForceMode.Impulse);

		// If not moving forward, apply overall drag.
		if (!attemptingForward && externalForce.sqrMagnitude < Helper.Epsilon)
		{
			body.velocity *= stats.overallDrag;
		}

		var forwardVelocity = Vector3.Project(body.velocity, transform.up);
		var sideVelocity = body.velocity - forwardVelocity;

		// If moving forward apply side-only drag to better align with direction we want to be going.
		if (attemptingForward)
		{
			sideVelocity *= stats.sideDrag;
		}

		body.velocity = forwardVelocity + sideVelocity;

		externalForce = Vector3.zero;

		// Stay on the plane
		body.velocity = new Vector3(body.velocity.x, body.velocity.y, 0);
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

	}

	private Vector3 ConstrainMove(Vector3 attemptedMove)
	{
		// TODO Check for flags that could prevent or force movement.

		var constrainedMove = Vector3.Project(attemptedMove, transform.up);
		if (Vector3.Dot(constrainedMove, transform.up) < 0)
		{
			constrainedMove = Vector3.zero;
		}
		return constrainedMove;
	}

	private void ApplyExternalForce(Vector3 force, bool forwardParallel = false)
	{
		ApplyExternalForce(force, PlaneComponent.All, forwardParallel);
	}

	private void ApplyExternalForce(Vector3 force, PlaneComponent planeComponent, bool forwardParallel = false)
	{
		var relevantForce = force;
		switch (planeComponent)
		{
			case PlaneComponent.Plane:
				relevantForce = OnPlane(force);
				break;
			case PlaneComponent.Normal:
				relevantForce = OnPlaneNormal(force);
				break;
		}

		if (forwardParallel)
		{
			relevantForce = Vector3.Project(relevantForce, transform.forward);
		}

		externalForce += relevantForce;
	}

	private Vector3 OnPlaneNormal(Vector3 toProject)
	{
		return Vector3.Project(toProject, up);
	}

	private Vector3 OnPlane(Vector3 toProject)
	{
		return toProject - OnPlaneNormal(toProject);
	}

	public void SetStats(MoveStats newStats)
	{
		stats = newStats;
	}

	public void ResetStats()
	{
		stats = defaultStats;
	}
}

[System.Serializable]
public class MoveStats
{
	public float maxSpeed = 10f;
	public float acceleration = 100f;

	[Tooltip("Degrees per Second")]
	public float turnSpeed = 180f;

	public float overallDrag = 0.6f;
	public float sideDrag = 0.6f;
}