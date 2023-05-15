using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimationScript : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        animator = GetComponent<Animator>();
        if (other.CompareTag("Player"))
        {
            //activate = true;
            //Debug.Log("Spike trap activated");
            animator.SetBool("Activate", true);
        }
    }
}
