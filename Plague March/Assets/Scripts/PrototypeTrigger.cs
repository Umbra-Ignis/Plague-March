using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeTrigger : MonoBehaviour {

    public GameObject Ai;
    private AudioSource Audio;
    public AudioClip Clip;


	// Use this for initialization
	void Start ()
    {  
        Audio = GetComponent<AudioSource>();  
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (!Audio.isPlaying)
        {
            Ai.SetActive(true);
            Audio.PlayOneShot(Clip);
        }
    }
}
