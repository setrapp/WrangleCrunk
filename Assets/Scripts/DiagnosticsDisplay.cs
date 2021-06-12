using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class DiagnosticsDisplay : MonoBehaviour
	{
		[SerializeField] private GameObject display;
		public bool visible = true;

		void Start()
		{
			display.SetActive(false);
		}

		private void Update()
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				if (display.activeSelf != visible)
				{
					display.SetActive(visible);
				}
			}
		}
	}
}