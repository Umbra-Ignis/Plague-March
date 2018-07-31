using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Crouching_Adrian : MonoBehaviour {

    private Animator animator;
    private CharacterController CharController;
    float Crouching;
    float NotCrouching;
    float timer = 0;

    

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        CharController = GetComponent<CharacterController>();
        Crouching = 0.666667f;
        NotCrouching = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += 0.5f *Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            CharController.center = new Vector3(0, 0.5f, 0);
            CharController.height = 1;
            animator.SetBool("IsCrouching", true);
            animator.SetFloat("Reverse", 1);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            CharController.center = new Vector3(0, 1, 0);
            CharController.height = 2;
            animator.SetBool("IsCrouching", false);
            animator.SetFloat("Reverse", -1.5f);
        }
	}
    
}
