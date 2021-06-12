using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
	public float Weight = 1;
	private SteeredMover mover = null;

	void Start()
	{
		mover = GetComponentInParent<SteeredMover>();
		if (mover != null)
		{
			mover.RegisterSteering(this);
		}
	}

	private void OnDestroy()
	{
		if (mover != null)
		{
			mover.UnregisterSteering(this);
		}
	}

	/// <summary>
	/// Determine the desired location to steer towards.
	/// </summary>
	/// <returns>Destination relative to current position, zero vector is no change.</returns>
	public abstract Vector3 ComputeDestinationRelative();
}
