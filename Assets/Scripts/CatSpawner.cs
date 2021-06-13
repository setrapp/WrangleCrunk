using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public GameObject cat;
    [SerializeField] private float priority = 1;
    public float Priority => priority;

    [SerializeField] private int spawnsRemaining = 5;
    public bool CanSpawn => spawnsRemaining > 0;

    public void SpawnCat()
    {
        Instantiate(cat, this.transform.position, Quaternion.identity);
        spawnsRemaining--;
    }
}
