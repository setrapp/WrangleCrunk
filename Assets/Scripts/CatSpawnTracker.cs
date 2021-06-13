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

    public float spawnRate = 1f;

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
            yield return new WaitForSeconds(spawnRate);
        }

        yield return null;
    }

    private void SpawnACat()
    {
        var sumChance = 0f;
        for (int i = 0; i < spawners.Count; i++)
        {
            var spawn = spawners[i];
            if (spawn.CanSpawn)
            {
                sumChance += spawn.Priority;
            }
        }

        var pick = Random.Range(0, sumChance);
        var sumPick = 0f;
        for (int i = 0; i < spawners.Count; i++)
        {
            var spawn = spawners[i];
            if (spawn.CanSpawn && sumPick >= pick)
            {
                spawn.SpawnCat();
                currentCatNumber += 1;
                break;
            }

            sumPick += spawn.Priority;
        }
    }
}
