//========================================================================================
//RockThrower_Adrian
//
//Functionality: Used to manage the throwing of the rock from the player
//
//Author: Adrian P
//========================================================================================
using UnityEngine;

public class RockThrower_Adrian : MonoBehaviour
{
    //Force
    public float throwForce = 40.0f;
    [Space]
    //GameObjects
    public GameObject RockPrefab;
    public GameObject spawnPoint;
    
    //movement script
    private Movement_Adrian moveScript;
    //Trajectory_Simulation
    private Trajectory_Simulation sim;
    //Velocity
    private Vector3 velocity;

    private void Start()
    {
        moveScript = GetComponent<Movement_Adrian>();
        sim = spawnPoint.GetComponent<Trajectory_Simulation>();
        velocity = Vector3.zero;
    }

    private void Update()
    {
        velocity = sim.getVelocity();

        // Aims depending on rock count
        if (Input.GetMouseButtonDown(0) && moveScript.GetRockCount() >= 0)
        {
            AimRock();
        }
        // Throws depending on rock count
        if (Input.GetMouseButtonUp(0) && moveScript.GetRockCount() >= 0)
        {
            ThrowRock();
        }
    }
    

    void ThrowRock()
    {
        if (moveScript.GetRockCount() >= 1)
        {
            // Instantiates rock being thrown
            GameObject rock = Instantiate(RockPrefab, spawnPoint.transform.position, Camera.main.transform.localRotation);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            rb.velocity = velocity;
            moveScript.SubtractRockCount();
            moveScript.rockThrown();
        }
    }

    void AimRock()
    {
        if (moveScript.GetRockCount() > 0)
        {
            //Stops movement
            moveScript.stopCharacter();
        }
    }

    public float GetAngle()
    {
        //Gets Angle of arc
        return Mathf.Acos(Vector3.Dot(Camera.main.transform.forward, transform.forward));
    }

}
