using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower_Adrian : MonoBehaviour
{
    public float throwForce = 40.0f;
    public GameObject RockPrefab;
    public GameObject spawnPoint;
    private Movement_Adrian moveScript;
    private Trajectory_Simulation sim;
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
        Debug.Log("got velocity");

        if (Input.GetMouseButtonDown(0) && moveScript.GetRockCount() >= 0)
        {
            AimRock();
        }

        if (Input.GetMouseButtonUp(0) && moveScript.GetRockCount() >= 0)
        {
            ThrowRock();
        }
    }
    

    void ThrowRock()
    {
        if (moveScript.GetRockCount() >= 1)
        {

            GameObject rock = Instantiate(RockPrefab, spawnPoint.transform.position, Camera.main.transform.localRotation);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            //rb.AddForce((Camera.main.transform.forward + Camera.main.transform.up) * throwForce, ForceMode.VelocityChange);
            rb.velocity = velocity;
            moveScript.SubtractRockCount();
            moveScript.rockThrown();
        }
    }

    void AimRock()
    {
        if (moveScript.GetRockCount() > 0)
        {
            Debug.Log("!!!!!");
            moveScript.stopCharacter();
        }
    }

    public float GetAngle()
    {
        return Mathf.Acos(Vector3.Dot(Camera.main.transform.forward, transform.forward));
    }

}
