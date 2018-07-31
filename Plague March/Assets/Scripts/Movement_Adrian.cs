using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Must Have a CharacterController
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]


public class Movement_Adrian : MonoBehaviour {

    //Gets Animator
    Animator animator;
    //Gets Character Controller
    CharacterController CharControler;
    //Gets Players Speed
    public float m_speed;
    //Gets Gravity
    public float m_Gravity;
    //Rotation Speed
    public float m_rotationSpeed;
    //Gets Mouse Sensitivity
    public float mouseSensitivity = 100.0f;
    //Gravity Modifier
    public float gravityModifier = -1f;
    //Velocity
    protected Vector3 velocity;
    //Rock Count
    public int rockCount;
    //Move Direction
    private Vector3 moveDirection = Vector3.zero;


    // Use this for initialization
    void Start () {

        //Presets speed
        m_speed = 0;
        //Gravity
        m_Gravity = 20;
        //Gets character controller Component
        CharControler = GetComponent<CharacterController>();
        //Sets Rotation Speed
        m_rotationSpeed = 0;
        //Gets Animator
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        //Sets the cursor locked and cannot not be seen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        if (CharControler.isGrounded)
        {
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= m_speed;

        }
        moveDirection.y -= m_Gravity * Time.deltaTime;
        CharControler.Move(moveDirection * Time.deltaTime);

        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        //Forward movement Animation
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("IsMovingForward", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("IsMovingForward", false);
        }


        //Grounded checks
        if (CharControler.isGrounded)
        {
            Debug.Log("Grounded");
            m_speed = 10;
        }
        else
        {
            Debug.Log("Not Grounded");
            m_speed = 0;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            Debug.Log("Rock");

            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E");

                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                rockCount++;
            }
        }
    }

    public int GetRockCount() { return rockCount; }
    public void SubtractRockCount() { rockCount--; }
}
