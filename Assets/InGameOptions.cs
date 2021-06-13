using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptions : MonoBehaviour
{

    public Animator optionsAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Open();
        }
    }

    public void Open()
    {
        optionsAnimator.SetBool("OptionsModal", true);
    }

    private void LateUpdate()
    {
        Cursor.visible = true;
    }

    public void Quit()
    {
        optionsAnimator.SetTrigger("Quit");
    }

    public void ReturnToGame()
    {
        optionsAnimator.SetBool("OptionsModal", false);
    }
}
