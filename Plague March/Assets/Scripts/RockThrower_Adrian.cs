using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower_Adrian : MonoBehaviour {

    public float throwForce = 40f;
    public GameObject RockPrefab;
    public Transform spawnPoint;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            ThrowRock();
        }
	}

    void ThrowRock()
    {
        GameObject rock = Instantiate(RockPrefab, spawnPoint.position, Camera.main.transform.localRotation);
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.AddForce((Camera.main.transform.forward + transform.up) * throwForce, ForceMode.VelocityChange);
    }
}
