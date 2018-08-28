using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone_Joel : MonoBehaviour
{
    public AIMove_Joel AI;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.tag == ("Player"))
            AI.SetAlert();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform t = player.transform;
        Vector3 tv = player.transform.position;
        AI.ApproachLastPos(tv);
        AI.SetPatrol();
    }
}
