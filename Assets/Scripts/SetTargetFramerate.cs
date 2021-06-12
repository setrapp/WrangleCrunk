using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
	public class SetTargetFramerate : MonoBehaviour
	{
		[SerializeField] int framerate = 60;
		[SerializeField] private TextMeshProUGUI framerateDisplay;
		[SerializeField] private TextMeshProUGUI frameTimeDisplay;

		private float sumDT = 0;
		private int samples = 0;

		private void Start()
		{
			Application.targetFrameRate = framerate;
		}

		private void Update()
		{
			sumDT += Time.deltaTime;
			samples++;

			if (sumDT >= 1 && samples > 0)
			{
				var dt = sumDT / samples;
				frameTimeDisplay.text = $"{(((int)(dt * 100000)) / 100f)}";
				framerateDisplay.text = $"{(int)(1 / Time.deltaTime)}";
				sumDT = 0;
				samples = 0;
			}
		}
	}
}