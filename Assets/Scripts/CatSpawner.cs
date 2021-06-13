using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public GameObject cat;

    public void SpawnCat()
    {
        Instantiate(cat, this.transform.position, Quaternion.identity);
    }
}
