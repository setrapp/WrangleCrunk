using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismissDialog : MonoBehaviour
{

    private Animator animator;
    public bool quitOnDismiss = false;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (animator.GetBool("DialogOpen"))
            {
                Dismiss();
            }
        }
    }

    public void Dismiss()
    {
        if (quitOnDismiss)
        {
            animator.SetTrigger("Quit");
        }
        else
        {
            animator.SetBool("DialogOpen", false);
        }
    }
}
