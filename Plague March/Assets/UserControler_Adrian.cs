﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement_Adrian))]

public class UserControler_Adrian : MonoBehaviour {

    //Main Char Reference
    private Movement_Adrian m_Character;
    //Main camera Transform reference
    private Transform m_Cam;
    //Forward direction of camera
    private Vector3 m_CamForward;
    //Movement
    public Vector3 m_Move;
    //Jumping
    private bool m_Jump;


    // Use this for initialization
    void Start ()
    {
        // get the transform of the main camera
            if (Camera.main != null)
            m_Cam = Camera.main.transform;

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<Movement_Adrian>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //locks Cursor in Center of Screen and makes Non visable
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }

        //Gets Inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Sets Crouch Bool
        bool crouch = Input.GetKey(KeyCode.LeftControl);

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        // walk speed multiplier
<<<<<<< HEAD:Plague March/Assets/Scripts/UserControler_Adrian.cs
<<<<<<< HEAD
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("SPRINTING");
            m_Move /= 0.5f;
        }
=======
        m_Sprinting = Input.GetKey(KeyCode.LeftShift);
        
>>>>>>> parent of 20d72ef... Added new test house, materials and UI elements
=======
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= .5f;
>>>>>>> parent of 47ffb93... Movement Polish and camera polish:Plague March/Assets/UserControler_Adrian.cs

        // pass all parameters to the Movement control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
}