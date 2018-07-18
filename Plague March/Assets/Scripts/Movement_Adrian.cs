using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Must Have a Rigidbody
[RequireComponent(typeof(Rigidbody))]

//Must Have a CapsuleCollider
[RequireComponent(typeof(CapsuleCollider))]

public class Movement_Adrian : MonoBehaviour {

    public int m_speed;

    Rigidbody m_rb;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    public float turnSmoothing = 15f; // A smoothing value for turning the player



    // Use this for initialization
    void Start () {

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        m_speed = 10;
        m_rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;

        float straffe = Input.GetAxis("Horizontal") * m_speed;
        float translation = Input.GetAxis("Vertical") * m_speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        transform.Translate(straffe, 0, translation);

        Vector3 fVelocity = new Vector3(straffe, 0, translation) * m_speed;

        //ROTATION
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

 
    }
}
