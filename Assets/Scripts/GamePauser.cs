using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{

    private CatSpawnTracker cst;

    public AudioSource pausedMusic;
    public AudioSource unpausedMusic;
    private void Start()
    {
        cst = FindObjectOfType<CatSpawnTracker>();
    }

    public void Pause()
    {
        pausedMusic.volume = 1f;
        unpausedMusic.volume = 0f;

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
        pausedMusic.volume = 0f;
        unpausedMusic.volume = 1f;

        //le troll face
        List<SteeredMover> movers = new List<SteeredMover>(FindObjectsOfType<SteeredMover>());
        foreach (SteeredMover m in movers)
        {
            m.UnPause();
        }

        cst.paused = false;
    }
}
