using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
	[SerializeField] private float weight = 1;
	public float Weight => weight;
	private float defaultWeight = 1;
	private SteeredMover mover = null;

	void Start()
	{
		mover = GetComponentInParent<SteeredMover>();
		if (mover != null)
		{
			mover.RegisterSteering(this);
		}

		defaultWeight = Weight;
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

	protected abstract Vector3 computeDestinationRelative();

	/// <summary>
	/// Determine the desired location to steer towards.
	/// </summary>
	/// <returns>Destination relative to current position, zero vector is no change.</returns>
	public Vector3 ComputeDestinationRelative()
	{
		if (enabled && gameObject.activeSelf)
		{
			return computeDestinationRelative();
		}
		return Vector3.zero;
	}
}
