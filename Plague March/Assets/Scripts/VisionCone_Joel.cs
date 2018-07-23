using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone_Joel : MonoBehaviour
{
    public UnityStandardAssets.Characters.ThirdPerson.AICharacterControl AI;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        AI.SetAlert();
        //AI.StopAgent();
    }
}
