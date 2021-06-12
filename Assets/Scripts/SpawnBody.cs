using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class SpawnBody : MonoBehaviour
	{
		// TODO This list should not exist on every cat.
		[SerializeField] private BodyInfo[] possibleBodies = null;
		private GameObject body = null;

		void Awake()
		{
			var pick = Random.Range(0f, 1f);
			var sum = 0f;
			for (int i = 0; i < possibleBodies.Length; i++)
			{
				sum += possibleBodies[i].spawnChance;
				if (sum >= pick)
				{
					body = Instantiate(possibleBodies[i].body, transform);
					break;
				}
			}
		}
	}

	[Serializable]
	public class BodyInfo
	{
		public string name;
		public GameObject body;
		public float spawnChance;
	}
}