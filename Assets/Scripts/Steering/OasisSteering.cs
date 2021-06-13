

using DefaultNamespace;
using UnityEngine;

public class OasisSteering : SteeringBehavior
{
	private bool seekHome = false;
	[SerializeField] private FogOfWarCulling culler = null;
	[SerializeField] private float minDistance = 10;

	protected override (Vector3 destination, float weight) computeDestinationRelative()
	{
		if (!seekHome)
		{
			return (Vector3.zero, 0);
		}

		var toTarget = Nexus.Instance.transform.position - transform.position;
		toTarget.z = 0;

		if (toTarget.sqrMagnitude < minDistance * minDistance)
		{
			return (Vector3.zero, 0);
		}
		return (toTarget, weight);
	}

	public void SeekHome()
	{
		seekHome = true;
		FindObjectOfType<CatSpawnTracker>().DecrementCurrentCats();
		if (culler != null)
		{
			culler.RevealForever();
		}

		Nexus.Instance.AddToTheFamily();
	}
}
