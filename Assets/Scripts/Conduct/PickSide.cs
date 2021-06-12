using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PickSide : MonoBehaviour
{
	enum Facing
	{
		None = 0,
		Up = 1,
		Right = 2,
		Down = 3,
		Left = 4
	}

	private Animator anim = null;
	private SteeredMover mover;
	private Facing facing = Facing.None;

	private void Start()
	{
		anim = GetComponent<Animator>();
		mover = GetComponentInParent<SteeredMover>();
	}

	private void Update()
	{
		var compass = mover.Compass;
		if (Mathf.Abs(compass.up.x) > Mathf.Abs(compass.up.y))
		{
			if (compass.up.x > 0)
			{
				if (facing != Facing.Right)
				{
					anim.SetTrigger("Walk_Right");
					facing = Facing.Right;
				}
			}
			else
			{
				if (facing != Facing.Left)
				{
					anim.SetTrigger("Walk_Left");
					facing = Facing.Left;
				}
			}
		}
		else
		{
			if (compass.up.y > 0)
			{
				if (facing != Facing.Up)
				{
					anim.SetTrigger("Walk_Up");
					facing = Facing.Up;
				}
			}
			else
			{
				if (facing != Facing.Down)
				{
					anim.SetTrigger("Walk_Down");
					facing = Facing.Down;
				}
			}
		}
	}
}
