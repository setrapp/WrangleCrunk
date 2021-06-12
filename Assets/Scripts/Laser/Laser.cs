using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
	public static Laser Instance { get; private set; }

	[SerializeField] private GameObject dot;
	private bool activated = false;
	public bool Activated => activated;

	private float charge = 0;
	[SerializeField] private float maxCharge = 100;
	[SerializeField] private float chargeRate = 50;
	[SerializeField] private float dischargeRate = 10;

	[SerializeField] private UnityEvent OnActivate = null;
	[SerializeField] private UnityEvent OnDeactivate = null;

	private BatteryLifeGradient batteryLifeGradient;

	private void Start()
	{
		Instance = this;
		charge = maxCharge;
		batteryLifeGradient = FindObjectOfType<BatteryLifeGradient>();
	}

	private void Update()
	{
		if ((Input.GetMouseButton(0) || Input.touches.Length > 0))
		{
			if (!activated)
			{
				activated = true;
				OnActivate.Invoke();
			}

			setCharge(Mathf.Max(charge - (dischargeRate * Time.deltaTime), 0));
		}
		else
		{
			if (activated)
			{
				activated = false;
				OnDeactivate.Invoke();
			}

			setCharge(Mathf.Min(charge + (chargeRate * Time.deltaTime), maxCharge));
		}
	}

	private void setCharge(float val)
	{
		charge = val;
		if(batteryLifeGradient != null)
        {
			batteryLifeGradient.SetFill(val / maxCharge);
        }
	}
}
