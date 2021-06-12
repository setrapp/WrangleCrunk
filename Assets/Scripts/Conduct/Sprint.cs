using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SteeredMover))]
public class Sprint : MonoBehaviour
{
	[SerializeField] private MoveStats sprintStats;
	[SerializeField] private float sprintChance = 1;
	private bool sprinting = false;
	private float defaultSprintChance = 1;
	[SerializeField] private float walkUpdateDelay = 1;
	[SerializeField] private float sprintUpdateDelay = 0.5f;
	private SteeredMover mover = null;

	private float untilChange = 0;

	private void Start()
	{
		mover = GetComponent<SteeredMover>();
		defaultSprintChance = sprintChance;
	}

	private void Update()
	{
		untilChange -= Time.deltaTime;
		if (untilChange <= 0)
		{
			CheckSprint();
		}
	}

	public void CheckSprint()
	{
		bool shouldSprint = Random.Range(0f, 1f) < sprintChance;
		if (shouldSprint)
		{
			if (!sprinting)
			{
				sprinting = true;
				mover.SetStats(sprintStats);
			}
		}
		else
		{
			if (sprinting)
			{
				sprinting = false;
				mover.ResetStats();
			}
		}

		untilChange = sprinting ? sprintUpdateDelay : walkUpdateDelay;
	}

	public void SetSprintChance(float newChance)
	{
		sprintChance = newChance;
	}

	public void ResetSprintChance()
	{
		sprintChance = defaultSprintChance;
	}
}
