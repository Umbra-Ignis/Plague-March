//========================================================================================
//TriggerBoxAudio
//
//Functionality: Used to play certain audio lines upon entering attached trigger boxes
//
//Author: Adrian P
//========================================================================================
using UnityEngine;

public class TriggerBoxAudio : MonoBehaviour
{
    //Audio Bools For Certain Areas check inspector for selection
    public bool m_bStopAndPlaySound;
    //Audio Bools For Certain Areas check inspector for selection
    public bool m_bWalkThroughSound;
    //Play audio with new camera
    public bool m_bNewCameraSound;
    //Audio Bool For Church
    public bool m_bChurchCameraLight;

    //Start Timer
    bool m_bStartTimer = false;

    bool m_bIsPlaying = false;

    //Audio manager setup
    new AudioSource audio;

    //New Camera
    public Camera NewCamera = null;

    //Point Light For Church
    public new GameObject light = null;

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

    private void OnTriggerStay(Collider other)
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

        if (m_bNewCameraSound)
        {
            if (NewCamera != null)
            {
                NewCamera.enabled = true;
            }

            if (NewCamera.enabled == true && !m_bIsPlaying)
            {
                m_bStartTimer = true;
                Player.SoundStop();
                m_fTimer = AudioToBePlayed.length;
                audio.PlayOneShot(AudioToBePlayed);
                m_bIsPlaying = true;
            }

            if (!m_bStartTimer)
            {
                if (NewCamera != null)
                {
                    NewCamera.enabled = false;
                    m_bNewCameraSound = false;
                }
            }
        }
        if (m_bChurchCameraLight)
        {
            if (NewCamera != null)
            {
                NewCamera.enabled = true;
            }

            if (NewCamera.enabled == true && !m_bIsPlaying)
            {
                if (light != null)
                {
                    light.SetActive(true);
                }
                m_bStartTimer = true;
                Player.SoundStop();
                m_fTimer = AudioToBePlayed.length;
                audio.PlayOneShot(AudioToBePlayed);
                m_bIsPlaying = true;
            }

            if (!m_bStartTimer)
            {
                if (NewCamera != null)
                {
                    if (light != null)
                    {
                        light.SetActive(false);
                    }
                    NewCamera.enabled = false;
                    m_bNewCameraSound = false;
                }
            }
        }

    }
}
