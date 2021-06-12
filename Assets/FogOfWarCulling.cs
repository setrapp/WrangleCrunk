using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarCulling : MonoBehaviour
{
    public float lineOfSight;
    public SpriteRenderer mainSprite;
    public SpriteRenderer minimapSprite;

    private bool spriteRevealed = false;
    
    private Transform playerTransform;

    private void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if(obj != null)
        {
            playerTransform = obj.transform;
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
        mainSprite.enabled = false;
        minimapSprite.enabled = false;
        spriteRevealed = false;
    }

    private void reveal()
    {
        mainSprite.enabled = true;
        minimapSprite.enabled = true;
        spriteRevealed = true;
    }
}
