using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawnTracker : MonoBehaviour
{

    public int targetCatNumber;
    public bool spawning = true;

    private int currentCatNumber;

    private List<CatSpawner> spawners;

    public bool paused = false;

    private void Start()
    {
        spawners = new List<CatSpawner>(FindObjectsOfType<CatSpawner>());
        StartCoroutine(HandleCatSpawning());
    }

    public void DecrementCurrentCats()
    {
        currentCatNumber -= 1;
    }

    private IEnumerator HandleCatSpawning()
    {
        while (spawning)
        {
            if(currentCatNumber < targetCatNumber && !paused)
            {
                SpawnACat();
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private void SpawnACat()
    {
        currentCatNumber += 1;
        CatSpawner spawner = spawners[Random.Range(0, spawners.Count - 1)];
        spawner.SpawnCat();
    }
}
