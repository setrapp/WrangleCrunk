using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
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
			untilChange = changeDelay;
			var changeAngle = Random.Range(-maxChangeAngle, maxChangeAngle);
			cachedDestination = Quaternion.Euler(0, 0, changeAngle) * transform.up;
			cachedDestination *= destinationDistance;
		}

		return cachedDestination;
	}
}
