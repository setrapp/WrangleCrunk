using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HerdSteering : SteeringBehavior
{
	[SerializeField] private float minStrength = 1;
	[SerializeField] private float maxStrength = 10;
	public float HerdStrength { get; private set; }
	[SerializeField] private float herdChance = 1;

	[SerializeField] private float unherdDelay = 3;
	[SerializeField] private float unherdChance = 0.5f;
	private float untilUnherd = 0;


	public HerdSteering herdBoss = null;
	public HerdSteering HerdBoss
	{
		get
		{
			if (herdBoss == null)
			{
				herdBoss = this;
			}

			return herdBoss;
		}
	}
	public List<HerdSteering> herdees = new List<HerdSteering>();

	void Awake()
	{
		HerdStrength = Random.Range(minStrength, maxStrength);
		herdBoss = this;
	}

	protected override (Vector3, float) computeDestinationRelative()
	{

		// TODO actually make herding.
		if (HerdBoss == this)
		{
			return (Vector3.zero, 0);
		}
		else
		{
			untilUnherd -= Time.deltaTime;

			if (untilUnherd <= 0)
			{
				if (Random.Range(0f, 1f) < unherdChance)
				{
					// Break away from the herd and be your own cat.
					herdBoss = this;
				}
				untilUnherd = unherdDelay;
			}

			return (HerdBoss.transform.position - transform.position, weight);
		}
	}

	public void MetHerdee(HerdSteering other)
	{
		if (other == null || other.HerdBoss == HerdBoss || Random.Range(0, 1) > herdChance)
		{
			return;
		}

		if (herdBoss.HerdStrength < other.HerdBoss.HerdStrength + Helper.Epsilon)
		{
			other.HerdBoss.AttemptAddHerdee(this);

			if (herdees.Count > 0)
			{
				for (int i = 0; i < herdees.Count; i++)
				{
					other.HerdBoss.AttemptAddHerdee(herdees[i]);
				}
				herdees.Clear();
			}
		}
	}

	public bool AttemptAddHerdee(HerdSteering newBlood)
	{
		if (newBlood == null || herdees.Count >= HerdStrength)
		{
			return false;
		}

		herdees.Add(newBlood);
		newBlood.JoinHerd(this);
		return true;
	}

	public void JoinHerd(HerdSteering herdBoss)
	{
		this.herdBoss = herdBoss;
		untilUnherd = unherdDelay;
	}
}
