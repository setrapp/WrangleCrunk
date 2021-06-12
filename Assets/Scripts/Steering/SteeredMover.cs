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

	// TODO This might be expensive for many instances. Maybe just register listeners without events.
	public UnityEvent OnMoveBegin = null;
	public UnityEvent OnMoveEnd = null;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		stats = defaultStats;
	}

	private void FixedUpdate()
	{
		// TODO remove controls from input
		//var forwardInput = Input.GetAxis("Vertical");
		//var sideInput = Input.GetAxis("Horizontal");

		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var toMouse = mousePos - transform.position;


		if (toMouse.sqrMagnitude > Helper.Epsilon)//Mathf.Abs(sideInput) > Helper.Epsilon || Mathf.Abs(forwardInput) > Helper.Epsilon)
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

		//var internalForce = ((transform.right * sideInput) + (transform.up * forwardInput)).normalized * stats.acceleration;

		if (toMouse.sqrMagnitude > 0)
		{
			var lookAt = toMouse.normalized;
			var angle = Mathf.Acos(Vector3.Dot(transform.up, lookAt)) * Mathf.Rad2Deg;
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
		}

		var moveForward = ConstrainMove(toMouse);
		bool attemptingMove = moveForward.sqrMagnitude > Helper.Epsilon;
		if (attemptingMove)
		{
			//Debug.Log(internalForce);
			body.AddForce(moveForward);
		}

		if (body.velocity.sqrMagnitude > stats.maxSpeed * stats.maxSpeed)
		{
			body.velocity = body.velocity.normalized * stats.maxSpeed;
		}

		body.AddForce(externalForce, ForceMode.Impulse);

		// If not moving forward, apply overall drag.
		if (!attemptingMove && externalForce.sqrMagnitude < Helper.Epsilon)
		{
			body.velocity *= stats.overallDrag;
		}

		var forwardVelocity = Vector3.Project(body.velocity, transform.up);
		var sideVelocity = body.velocity - forwardVelocity;

		// If moving forward apply side-only drag to better align with direction we want to be going.
		if (attemptingMove)
		{
			sideVelocity *= stats.sideDrag;
		}

		body.velocity = forwardVelocity + sideVelocity;

		externalForce = Vector3.zero;
	}

	private Vector3 ConstrainMove(Vector3 attemptedMove)
	{
		// TODO Check for flags that could prevent or force movement.

		var constrainedMove = Vector3.Project(attemptedMove, transform.up);
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