using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarCulling : MonoBehaviour
{
    public float lineOfSight;
    public GameObject hideOnCull;
    public SpriteRenderer minimapSprite;

    private bool spriteRevealed = false;

    private Transform playerTransform;

    private void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if(obj != null)
        {
            playerTransform = obj.transform;
        } else
        {
            Debug.LogWarning("Fog Of War Culling couldn't find player");
        }

        if(withinLineOfSight())
        {
            reveal();
        } else
        {
            cull();
        }
    }

    void Update()
    {
        if(spriteRevealed && !withinLineOfSight())
        {
            cull();
        }
        else if(!spriteRevealed && withinLineOfSight())
        {
            reveal();
        }
    }

    private bool withinLineOfSight()
    {
        return Vector3.SqrMagnitude(this.transform.position - playerTransform.position) < lineOfSight * lineOfSight;
    }

    private void cull()
    {
        hideOnCull.SetActive(false);
        minimapSprite.enabled = false;
        spriteRevealed = false;
    }

    private void reveal()
    {
        hideOnCull.SetActive(true);
        minimapSprite.enabled = true;
        spriteRevealed = true;
    }

    public void RevealForever()
    {
        reveal();
        enabled = false;
    }
}
