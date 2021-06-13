

using UnityEngine;

public class OasisSteering : SteeringBehavior
{
	private Transform target;

	protected override (Vector3 destination, float weight) computeDestinationRelative()
	{
		if (target == null)
		{
			return (Vector3.zero, 0);
		}

		var toTarget = target.position - transform.position;
		toTarget.z = 0;
		return (toTarget, weight);
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}
