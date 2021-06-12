using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class WanderSteering : SteeringBehavior
{
	[SerializeField] float maxChangeAngle = 45;
	[SerializeField] private float destinationDistance = 10;
	[SerializeField] private float changeDelay = 1;
	private float untilChange = 0;
	private Vector3 cachedDestination = Vector3.zero;

	protected override Vector3 computeDestinationRelative()
	{
		untilChange -= Time.deltaTime;
		if (untilChange <= 0)
		{
			UpdateDestination();
		}

		return cachedDestination;
	}

	private void UpdateDestination(float angleFactor = 1)
	{
		var changeAngle = Random.Range(-maxChangeAngle * angleFactor, maxChangeAngle * angleFactor);
		cachedDestination = Quaternion.Euler(0, 0, changeAngle) * transform.up;
		cachedDestination *= destinationDistance;
		untilChange = changeDelay;

	}

	private void OnEnable()
	{
		// Reset wander destination to current heading when woken up, to prevent always turning around.
		cachedDestination =  mover.Compass.up * destinationDistance;
		UpdateDestination(0.25f);
	}
}