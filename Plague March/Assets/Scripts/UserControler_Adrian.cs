using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement_Adrian))]

public class UserControler_Adrian : MonoBehaviour
{

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
    //Sprinting
    private bool m_Sprinting;

    private bool haveKey1;
    private bool haveKey2;
    private bool haveKey3;
    private bool haveKey4;

    // Use this for initialization
    void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
            m_Cam = Camera.main.transform;

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<Movement_Adrian>();
        //locks Cursor in Center of Screen and makes Non visable
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;

        haveKey1 = false;
        haveKey2 = false;
        haveKey3 = false;
        haveKey4 = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        m_Sprinting = Input.GetKey(KeyCode.LeftShift);


        // pass all parameters to the Movement control script
        m_Character.Move(m_Move, crouch, m_Jump, m_Sprinting);
        m_Jump = false;
    }

    public void obtainedKey(int keyNum)
    {
        if (keyNum == 1)
        {
            haveKey1 = true;
        }

        else if (keyNum == 2)
        {
            haveKey2 = true;
        }

        else if (keyNum == 3)
        {
            haveKey3 = true;
        }

        else if (keyNum == 4)
        {
            haveKey4 = true;
        }
    }

    public bool haveKey(int keyNum)
    {
        if(keyNum == 1)
        {
            return haveKey1;
        }

        else if(keyNum == 2)
        {
            return haveKey2;
        }

        else if (keyNum == 3)
        {
            return haveKey3;
        }

        else if (keyNum == 4)
        {
            return haveKey4;
        }

        else
        {
            return false;
        }
    }
}