using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
	public class Nexus : MonoBehaviour
	{
		public static Nexus Instance;

		[SerializeField] int winCount = 10;
		private int currentCount = 0;
		[SerializeField] private TextMeshProUGUI currentCountText;
		[SerializeField] private TextMeshProUGUI winCountText;


		private void Awake()
		{
			Instance = this;
			winCountText.text = $"{winCount}";
			currentCountText.text = "0";
		}

		public void AddToTheFamily()
		{
			currentCount++;
			currentCountText.text = $"{winCount}";
		}
	}
}