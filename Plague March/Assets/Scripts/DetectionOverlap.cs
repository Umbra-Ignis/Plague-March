using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionOverlap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 10);

        DebugExtension.DebugWireSphere(transform.position, 10, 10);
    }
}
