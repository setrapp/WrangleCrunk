using System;
using UnityEngine;

public abstract class EscapeResponder : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			HandleEscape();
		}
	}

	protected abstract void HandleEscape();

}