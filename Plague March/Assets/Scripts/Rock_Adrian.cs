using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Adrian : MonoBehaviour
{
    public float AlertRadius;

    public GameObject HitFloorEffect;

    public Movement_Adrian moveScript;

	// Use this for initialization
	void Start () 
    {
        moveScript = GetComponent<Movement_Adrian>();
    }
	
	// Update is called once per frame
	void Update (){}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("COLLISION!!!!!");
            Alert();
        }
    }

    void Alert()
    {
        //Instantiate(HitFloorEffect, transform.position, transform.rotation);

        Collider[] collider = Physics.OverlapSphere(transform.position, AlertRadius);

        for (int i = 0; i < collider.Length; i++)
        {
            Debug.Log("FOR LOOP");

            if (collider[i].gameObject.CompareTag("Enemy"))
            {
                Debug.Log("COLLIDER IF");
                DebugExtension.DebugWireSphere(collider[i].transform.position, 10, 10);
                collider[i].gameObject.GetComponent<AIMove_Joel>().SetPatrolPoint(transform);
            }
        }
        
        //Alert Enemy
        //Destroy(gameObject);
    }
}
