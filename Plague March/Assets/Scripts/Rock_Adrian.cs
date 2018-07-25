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
        }
	}
    
    void Alert()
    {
        //Instantiate(HitFloorEffect, transform.position, transform.rotation);

        Collider[] collider = Physics.OverlapSphere(transform.position, AlertRadius);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.CompareTag("Enemy"))
            {
                //collider[i].gameObject.GetComponent<AIMove_Joel>().
            }
        }
        

        //Alert Enemy
        //Destroy(gameObject);
    }
}
