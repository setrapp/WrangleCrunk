using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeredMover : MonoBehaviour
{
	enum PlaneComponent
	{
		All,
		Plane,
		Normal
	}

	private Rigidbody2D body = null;
	public Rigidbody2D Body => Body;

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

	[SerializeField] public Transform Compass = null;

	[NonSerialized] public Vector3 moveDirection = Vector3.up;


	public bool paused = false;

	// TODO This might be expensive for many instances. Maybe just register listeners without events.
	public UnityEvent OnMoveBegin = null;
	public UnityEvent OnMoveEnd = null;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		stats = defaultStats;
		if (Compass == null)
		{
			Compass = transform;
		}
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
				var modifiedWeight = steering.Weight;// * (1 - Mathf.Min(steeringRequest.sqrMagnitude / maxSqrDist, 1));
				weightSum += modifiedWeight;
				modifiedWeights[i] = modifiedWeight;
				cachedDestinations[i] = steeringRequest.normalized;
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

		if (destination.sqrMagnitude > Helper.Epsilon)
		{
			var lookAt = destination.normalized;

			// Arc Cosine is only define in [-1, 1] so prevent Dot Product from rounding error past that.
			var cos = Mathf.Min(Vector3.Dot(Compass.up, lookAt), 1);
			var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;

			if (Vector3.Dot(Compass.right, lookAt) > 0)
			{
				angle *= -1;
			}

			// If attempting to turn around completly, randomly choose a direction to turn.
			if (Mathf.Abs(Vector3.Dot(Compass.up, lookAt) + 1) < Helper.Epsilon
			    && Random.Range(0f, 1f) < 0.5f)
			{
				angle = -180;
			}

			var maxTurn = stats.turnSpeed * Time.deltaTime;
			if (Mathf.Abs(angle) > maxTurn)
			{
				angle = angle > 0 ? maxTurn : -maxTurn;
			}

			angle += Compass.rotation.eulerAngles.z;

			Compass.rotation = Quaternion.AngleAxis(angle, up);
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

		if (attemptingForward)
		{
			body.AddForce(moveForward * stats.acceleration);
			moveDirection = moveForward;
		}

		bool aboveMaxSpeed = body.velocity.sqrMagnitude > stats.maxSpeed * stats.maxSpeed;

		body.AddForce(externalForce, ForceMode2D.Impulse);

		// If not moving forward or moving too fast, apply overall drag.
		if ((!attemptingForward || aboveMaxSpeed) && externalForce.sqrMagnitude < Helper.Epsilon)
		{
			body.velocity *= stats.overallDrag;
		}

		var forwardVelocity = (Vector2)Vector3.Project(body.velocity, Compass.up);
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

		var constrainedMove = Vector3.Project(attemptedMove, Compass.up);
		if (Vector3.Dot(constrainedMove, Compass.up) < 0)
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
			relevantForce = Vector3.Project(relevantForce, Compass.up);
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

	public void Pause()
    {
		MoveStats s = new MoveStats();
		s.maxSpeed = 0f;
		s.acceleration = 0f;		
		SetStats(s);
    }

	public void UnPause()
    {
		MoveStats s = new MoveStats();
		SetStats(s);
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