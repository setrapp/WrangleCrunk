using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSteering : SteeringBehavior
{
	protected override Vector3 computeDestinationRelative()
	{
		var destination = Vector3.zero;
		var laser = Laser.Instance;

		if (FollowingLaser(laser))
		{
			var laserPos = laser.transform.position;
			destination = laserPos - transform.position;
		}

		return destination;
	}

	bool FollowingLaser(Laser laser)
	{
		// TODO Actually check if I'm trying to follow the laser
		return laser.Activated;
	}
}
