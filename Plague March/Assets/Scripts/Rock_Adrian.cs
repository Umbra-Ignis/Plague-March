using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Adrian : MonoBehaviour {

    

    public float AlertRadius;

    public GameObject HitFloorEffect;

    


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
       
            Alert();
        
	}
    
    void Alert()
    {
        //Instantiate(HitFloorEffect, transform.position, transform.rotation);

        Collider[] collider = Physics.OverlapSphere(transform.position, AlertRadius);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.CompareTag("Enemy"))
            {
                DebugExtension.DebugWireSphere(collider[i].transform.position);
                collider[i].gameObject.GetComponent<AIMove_Joel>().SetPatrolPoint(transform);
            }
        }
        

        //Alert Enemy
        //Destroy(gameObject);
    }
}
