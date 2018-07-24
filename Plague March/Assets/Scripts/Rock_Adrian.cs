using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Adrian : MonoBehaviour {

    public float delay = 3f;

    float countdown;

    bool hasHitFloor = false;

    public float AlertRadius;

    public GameObject HitFloorEffect;

	// Use this for initialization
	void Start () {
        countdown = delay;
	}
	
	// Update is called once per frame
	void Update ()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasHitFloor)
        {
            Alert();
            hasHitFloor = true;
        }
	}
    
    void Alert()
    {
        //Instantiate(HitFloorEffect, transform.position, transform.rotation);

        //Collider[] collider = Physics.OverlapSphere(transform.position, AlertRadius);
        

        //Alert Enemy
        Destroy(gameObject);
    }
}
