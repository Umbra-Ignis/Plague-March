using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxAudio : MonoBehaviour {

    //Audio Bools For Certain Areas check inspector for selection
    public bool m_bStopAndPlaySound;
    //Audio Bools For Certain Areas check inspector for selection
    public bool m_bWalkThroughSound;

    //Start Timer
    bool m_bStartTimer = false;

    //Audio manager setup
    AudioSource audio;

    //Gets Player to stop moving
    Movement_Adrian Player;

    //Audio Clips to be placed to corosponding areas
    public AudioClip AudioToBePlayed;

    //Audio Timer For Movement
    float m_fTimer; 

    private void Awake()
    {
        //Getting Audio Manager and source
        audio = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>();
    }

    private void Update()
    {
        if (m_bStartTimer)
        {   
            m_fTimer -= Time.deltaTime;

            if (m_fTimer <= 0)
            {
                m_fTimer = 0;
            }
            if (m_fTimer <= 0)
            {
                Player.SoundStart();
                m_bStartTimer = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_bStopAndPlaySound)
        {
            m_bStartTimer = true;
            Player.SoundStop();
            m_fTimer = AudioToBePlayed.length;
            audio.PlayOneShot(AudioToBePlayed);
            m_bStopAndPlaySound = false;
        }

        if (m_bWalkThroughSound)
        {
            m_bStartTimer = true;
            audio.PlayOneShot(AudioToBePlayed);
            m_bWalkThroughSound = false;
        }

    }


}
