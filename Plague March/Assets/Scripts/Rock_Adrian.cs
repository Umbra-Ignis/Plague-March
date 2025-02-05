﻿//========================================================================================
//Rock_Adrian
//
//Functionality: Script attached to the rock to alert the AI and to be picked up
//
//Author: Adrian P
//Altered by: Joel G
//========================================================================================
using UnityEngine;

public class Rock_Adrian : MonoBehaviour
{
    public float AlertRadius;
    public GameObject HitFloorEffect;
    private Rigidbody rb;
    public bool canPickup;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canPickup = false;
    }

    // Update is called once per frame
    void Update() { }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Alert();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            canPickup = true;
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
                DebugExtension.DebugWireSphere(collider[i].transform.position, AlertRadius, 10);
                collider[i].gameObject.GetComponent<AIMove_Joel>().ApproachRock(transform);
                collider[i].gameObject.GetComponent<AIMove_Joel>().RockThrowBools();
            }
        }
    }
}
