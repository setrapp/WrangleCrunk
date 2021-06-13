using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class CameraAndMouseSettings : MonoBehaviour
	{
		[SerializeField] private bool confineMouse = false;
		[SerializeField] private bool hideMouse = false;

		private void Start()
		{
			// Set mouse to center of screen
			Cursor.lockState = CursorLockMode.Locked;

#if !UNITY_EDITOR
			Cursor.lockState = confineMouse ? CursorLockMode.Confined : CursorLockMode.None;
#else
			Cursor.lockState = CursorLockMode.None;
#endif
		}
		void Update()
		{
			Cursor.visible = !hideMouse;
		}
	}
}