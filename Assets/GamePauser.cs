using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{

    private CatSpawnTracker cst;

    private void Start()
    {
        cst = FindObjectOfType<CatSpawnTracker>();
    }

    public void Pause()
    {
        //le troll face
        List<SteeredMover> movers = new List<SteeredMover>(FindObjectsOfType<SteeredMover>());
        foreach (SteeredMover m in movers)
        {
            m.Pause();
        }

        cst.paused = true;
    }

    public void UnPause()
    {
        //le troll face
        List<SteeredMover> movers = new List<SteeredMover>(FindObjectsOfType<SteeredMover>());
        foreach (SteeredMover m in movers)
        {
            m.UnPause();
        }

        cst.paused = false;
    }
}
