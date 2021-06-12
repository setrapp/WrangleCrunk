using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarCulling : MonoBehaviour
{
    public float lineOfSight;

    private bool spriteRevealed = false;
    private SpriteRenderer sprite;
    private Transform playerTransform;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
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
        sprite.enabled = false;
        spriteRevealed = false;
    }

    private void reveal()
    {
        sprite.enabled = true;
        spriteRevealed = true;
    }
}
