using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LaserSteering : SteeringBehavior
{
	private bool chasing = false;
	[SerializeField] private UnityEvent OnBeginChase = null;
	[SerializeField] private UnityEvent OnEndChase = null;
	[SerializeField] private float startChaseDistance = 20;
	[SerializeField] private float endChaseDistance = 40;
	[SerializeField] private float checkExcitedDelay = 0.5f;
	[SerializeField] private float checkBoredDelay = 1f;
	[SerializeField] private float excitedChance = 0.3f;
	[SerializeField] private float boredChance = 0.1f;

	private float untilExciteCheck = 0;
	private float untilBoredCheck = 0;

	private Vector3 lastKnownDot = Vector3.zero;
	private Coroutine returnWait = null;

	protected override (Vector3, float) computeDestinationRelative()
	{
		var destination = Vector3.zero;
		var laser = Laser.Instance;

		if (chasing)
		{
			untilBoredCheck -= Time.deltaTime;
		}

		if (ChasingLaser(laser))
		{
			var laserPos = lastKnownDot;
			if (laser.Activated)
			{
				laserPos = laser.transform.position;
			}

			destination = laserPos - transform.position;

			if (!chasing)
			{
				chasing = true;
				untilBoredCheck = checkBoredDelay;
				OnBeginChase.Invoke();
			}
		}
		else
		{
			if (chasing)
			{
				chasing = false;
				untilExciteCheck = checkExcitedDelay;
				OnEndChase.Invoke();
			}
		}

		return (destination, chasing ? weight : 0);
	}

	bool LaserActiveNearby()
	{
		var laser = Laser.Instance;
		if (!laser.Activated)
		{
			untilExciteCheck = checkExcitedDelay;
			return false;
		}

		var maxSqrDist = chasing ? endChaseDistance * endChaseDistance : startChaseDistance * startChaseDistance;
		return laser.Activated && (laser.transform.position - transform.position).sqrMagnitude < maxSqrDist;
	}

	bool ChasingLaser(Laser laser)
	{
		bool shouldChase = chasing;
		if (LaserActiveNearby())
		{
			untilExciteCheck -= Time.deltaTime;

			if (chasing)
			{
				if (untilBoredCheck <= 0)
				{
					shouldChase = Random.Range(0f, 1f) > boredChance;
					untilBoredCheck = checkBoredDelay;
				}
			}
			else
			{
				if (untilExciteCheck <= 0)
				{
					shouldChase = Random.Range(0f, 1f) < excitedChance;
					untilExciteCheck = checkExcitedDelay;
				}
			}
		}
		else
		{
			if (chasing)
			{
				/*if (returnWait == null)
				{
					returnWait = StartCoroutine(waitForReturn());
				}*/

				shouldChase = false;//Random.Range(0f, 1f) > boredChance;
				untilBoredCheck = checkBoredDelay;
			}
		}

		return shouldChase;
	}

	/*IEnumerator waitForReturn()
	{
		lastKnownDot = Laser.Instance.transform.position;
		yield return new WaitForSeconds(checkBoredDelay);
		if (!LaserActiveNearby())
		{
			chasing = false;
			untilExciteCheck = checkExcitedDelay;
		}
		yield break;
	}*/
}
