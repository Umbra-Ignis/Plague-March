//========================================================================================
//PermalockTrigger
//
//Functionality: Attached to a trigger box to permanently lock a door upon entering
//
//Author: Joel G
//========================================================================================
using UnityEngine;

public class PermalockTrigger : MonoBehaviour
{
    public GameObject doorTrigger;
    private DoorOpen_Joel script;

    // Use this for initialization
    void Start()
    {
        script = doorTrigger.GetComponent<DoorOpen_Joel>();
    }

    // Update is called once per frame
    void Update(){}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && script.opened)
        {
            script.CloseDoor();
        }
    }
}
