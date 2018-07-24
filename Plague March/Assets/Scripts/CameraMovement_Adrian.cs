using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_Adrian : MonoBehaviour {

    //Mouse look sensitivity
    public float mouseSensitivity = 100.0f;

    //Clamps angle min and max
    public float clampAngleMax;
    public float clampAngleMin;

    
    private float rotX = 0.0f; // rotation around the right/x axis
    public float turnSmoothing = 15f; // A smoothing value for turning the player
    

    // Use this for initialization
    void Start ()
    {
        //Gets the local Rotation
        Vector3 rot = transform.localRotation.eulerAngles;
        //Sets Local Rotation
        rotX = rot.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //ROTATION
        float mouseY = -Input.GetAxis("Mouse Y");
        
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        
        //Clamps camera angle
        rotX = Mathf.Clamp(rotX, -clampAngleMax, clampAngleMin);
        
        Quaternion localRotation = Quaternion.Euler(rotX, transform.parent.eulerAngles.y, 0.0f);
        
        //sets local calculations into the objects rotation
        transform.rotation = localRotation;

    }
}
