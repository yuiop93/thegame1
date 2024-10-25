using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("IsCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetBool("IsCrouching", false);
        }
    }
}
