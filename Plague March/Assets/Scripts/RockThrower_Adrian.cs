using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower_Adrian : MonoBehaviour
{
    public float throwForce = 40.0f;
    public GameObject RockPrefab;
    public Transform spawnPoint;
    private Movement_Adrian moveScript;

    private void Start()
    {
        moveScript = GetComponent<Movement_Adrian>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0) && moveScript.GetRockCount() >= -0)
        {
            ThrowRock();
        }
	}

    void ThrowRock()
    {
        if (moveScript.GetRockCount() >= 1)
        {

            GameObject rock = Instantiate(RockPrefab, spawnPoint.position, Camera.main.transform.localRotation);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            rb.AddForce((Camera.main.transform.forward + transform.up) * throwForce, ForceMode.VelocityChange);
            moveScript.SubtractRockCount();
        }
    }
}
