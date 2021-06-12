using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class SetTargetFramerate : MonoBehaviour
	{
		[SerializeField] int framerate = 60;

		private void Start()
		{
			Application.targetFrameRate = framerate;
		}
	}
}