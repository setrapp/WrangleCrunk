using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
	[SerializeField] private Camera cam;
	private Vector3 initialPosition;

	void Start()
	{
		initialPosition = transform.position;
	}

	void Update()
	{
		var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialPosition.z);
		transform.position = cam.ScreenToWorldPoint(mousePos);
	}
}
