﻿//========================================================================================
//Teleport_Adrian
//
//Functionality: Used to teleport
//
//Author: Adrian P
//========================================================================================
using UnityEngine;

public class Teleport_Adrian : MonoBehaviour
{

    [Header("Codes value must match corresponding teleporter")]

    // To match the teleporters together
    public int code;
    //Time before next Teleport
    public float timeBeforeNextTp = 0;
    //Stops players from constantly teleporting
    float disableTimer = 0;
    //Loading Screen Timer
    float LoadingScreenTimer = 0;
    //Loading Screen Bool
    bool LoadingScreen;

    private void Start(){}

    private void Update()
    {
        //sets timer back to 0
        if (disableTimer > 0)
            disableTimer -= Time.deltaTime;

        if (LoadingScreen)
        {
            LoadingScreenTimer += Time.deltaTime;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // Add Feature && Input.GetKey(KeyCode.E)
        if (Input.GetKey(KeyCode.E))
        {
            LoadingScreen = true;
            //check if player and timer 0 before using teleporter pad
            if (other.gameObject.tag == "Player" && disableTimer <= 0 && LoadingScreenTimer >= 5)
            {
                LoadingScreen = false;
                

                //finds each teleporter pad to make sure they match
                foreach (Teleport_Adrian tp in FindObjectsOfType<Teleport_Adrian>())
                {
                    if (tp.code == code && tp != this)
                    {
                        //Resets timer to 2 seconds
                        tp.disableTimer = timeBeforeNextTp;
                        //Moves player positions
                        Vector3 Position = tp.gameObject.transform.position;
                        other.gameObject.transform.position = Position;

                        LoadingScreenTimer = 0;
                    }
                }
            }
        }
    }
}
