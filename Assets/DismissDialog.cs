using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismissDialog : MonoBehaviour
{

    private Animator animator;

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
        Debug.Log("Dismissing Dialog");
        animator.SetBool("DialogOpen", false);  
    }
}
