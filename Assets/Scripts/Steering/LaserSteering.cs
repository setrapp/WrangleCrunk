using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserSteering : SteeringBehavior
{
	private bool chasing = false;
	[SerializeField] private UnityEvent OnBeginChase = null;
	[SerializeField] private UnityEvent OnEndChase = null;


	protected override Vector3 computeDestinationRelative()
	{
		var destination = Vector3.zero;
		var laser = Laser.Instance;

		if (ChasingLaser(laser))
		{
			var laserPos = laser.transform.position;
			destination = laserPos - transform.position;

			if (!chasing)
			{
				chasing = true;
				OnBeginChase.Invoke();
			}
		}
		else
		{
			if (chasing)
			{
				chasing = false;
				OnEndChase.Invoke();
			}
		}

		return destination;
	}

	bool ChasingLaser(Laser laser)
	{
		// TODO Actually check if I'm trying to follow the laser
		return laser.Activated;
	}
}
