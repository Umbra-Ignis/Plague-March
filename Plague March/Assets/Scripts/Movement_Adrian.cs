using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Must Have a CharacterController
[RequireComponent(typeof(CharacterController))]


public class Movement_Adrian : MonoBehaviour {

    //Gets Character Controller
    CharacterController CharControler;
    //Gets Players Speed
    public int m_speed;
    //Gets Rotation Y
    private float rotY = 0.0f; // rotation around the up/y axis
    //Gets Mouse Sensitivity
    public float mouseSensitivity = 100.0f;
    //Gravity Modifier
    public float gravityModifier = -1f;
    //Velocity
    protected Vector3 velocity;
    //Rock Count
    public int rockCount;


    // Use this for initialization
    void Start () {

        //Presets speed
        m_speed = 10;
        //Gets character controller Component
        CharControler = GetComponent<CharacterController>();
        //Gets Rotation
        Vector3 rot = transform.localRotation.eulerAngles;
        //Sets rotation
        rotY = rot.y;
    }
	
	// Update is called once per frame
	void Update () {

        //Sets the cursor locked and cannot not be seen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        //WASD CONTROLS
        float straffe = Input.GetAxis("Horizontal") * m_speed;
        float translation = Input.GetAxis("Vertical") * m_speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;
        transform.Translate(straffe, 0, translation);

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
        
        //Gets x axis for look around
        float mousex = Input.GetAxis("Mouse X");
        
        rotY += mousex * mouseSensitivity * Time.deltaTime;
        
        Quaternion localRotation = Quaternion.Euler(0, rotY, 0.0f);

        //Sets local rotation of object
        transform.rotation = localRotation;
    }

    private void FixedUpdate()
    {
        //Gravity
        velocity += gravityModifier * Physics.gravity * Time.deltaTime;
        Vector3 deltaPosition = velocity * Time.deltaTime;
        Vector3 move = Vector3.up * deltaPosition.y;
        CharControler.Move(move);
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
