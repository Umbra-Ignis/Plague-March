using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Adrian : MonoBehaviour {

    [Header("Codes value must match corresponding teleporter")]

    // To match the teleporters together
    public int code;
    //Time before next Teleport
    public float timeBeforeNextTp = 0;
    //Stops players from constantly teleporting
    float disableTimer = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        //sets timer back to 0
        if (disableTimer > 0)
            disableTimer -= Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Add Feature && Input.GetKey(KeyCode.E)

        //check if player and timer 0 before using teleporter pad
        if (other.gameObject.tag == "Player" && disableTimer <= 0)
        {
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
                }
            }
        }
    }
}
