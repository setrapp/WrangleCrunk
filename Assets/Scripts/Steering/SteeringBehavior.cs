using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
	[SerializeField] protected float weight = 1;
	private float defaultWeight = 1;
	protected SteeredMover mover = null;

	void Start()
	{
		mover = GetComponentInParent<SteeredMover>();
		if (mover != null)
		{
			mover.RegisterSteering(this);
		}

		defaultWeight = weight;
	}

	private void OnDestroy()
	{
		if (mover != null)
		{
			mover.UnregisterSteering(this);
		}
	}

	public void SetWeight(float newWeight)
	{
		weight = newWeight;
	}

	public void ResetWeight()
	{
		weight = defaultWeight;
	}

	protected abstract (Vector3 destination, float weight) computeDestinationRelative();

	/// <summary>
	/// Determine the desired location to steer towards.
	/// </summary>
	/// <returns>Destination relative to current position, zero vector is no change.</returns>
	public (Vector3 destination, float weight) ComputeDestinationRelative()
	{
		if (enabled && gameObject.activeSelf)
		{
			return computeDestinationRelative();
		}
		return (Vector3.zero, 0);
	}
}
