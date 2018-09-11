using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone_Joel : MonoBehaviour
{
    public AIMove_Joel AI;
    Vector3 PlayersTransform;
    GameObject player;

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
        if(other.gameObject.tag == "Player")
        {
            AI.SetAlertTimer(0);
            player = GameObject.FindGameObjectWithTag("Player");
            PlayersTransform = player.transform.position;
            AI.ApproachLastPos(PlayersTransform);
            AI.SetPatrol();
        }
    }
}
