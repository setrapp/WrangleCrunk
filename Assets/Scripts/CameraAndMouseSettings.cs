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
#if !UNITY_EDITOR
			Cursor.lockState = confineMouse ? CursorLockMode.Confined : CursorLockMode.None;
#endif
		}
		void Update()
		{
			Cursor.visible = !hideMouse;
		}
	}
}