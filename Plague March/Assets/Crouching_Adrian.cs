using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Crouching_Adrian : MonoBehaviour {

    private Animator animator;
    private CharacterController CharController;

    

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        CharController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            CharController.center = new Vector3(0, 0.5f, 0);
            CharController.height = 1;
            animator.SetBool("Crouch", true);
           
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            CharController.center = new Vector3(0, 1, 0);
            CharController.height = 2;
            animator.SetBool("Crouch", false);
            
        }
	}
    
}
