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
		[SerializeField] private TextMeshProUGUI countText;


		private void Awake()
		{
			Instance = this;
			countText.text = $"{0 / winCount}";
		}

		public void AddToTheFamily()
		{
			currentCount++;
			countText.text = $"{currentCount / winCount}";

		}
	}
}