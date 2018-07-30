using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone_Joel : MonoBehaviour
{
    public AIMove_Joel AI;

    public float timer = 0.0f;
    // Use this for initialization
    void Start (){}
	
	// Update is called once per frame
	void Update (){}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        AI.SetAlert();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        AI.SetPatrol();
    }
}
