using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
	public class Nexus : MonoBehaviour
	{
		public static Nexus Instance;

		[SerializeField] int winCount = 10;
		private int currentCount = 0;
		[SerializeField] private TextMeshProUGUI countText;

		public UnityEvent onFamily = null;

		private bool won = false;
		public UnityEvent onWin = null;


		private void Awake()
		{
			Instance = this;
			countText.text = $"0 / {winCount}";
		}

		public void AddToTheFamily()
		{
			currentCount++;
			countText.text = $"{currentCount} / {winCount}";
			onFamily.Invoke();

			if (currentCount >= winCount && !won)
			{
				won = true;
				onWin.Invoke();
			}
		}
	}
}