//========================================================================================
//TavernKeepTrigger
//
//Functionality: Called upon entering the tavern to trigger the "cut-scene" event
//
//Author: Adrian P
//========================================================================================
using UnityEngine;

public class Tavern_Keep_Trigger : MonoBehaviour
{

    //Public animator
    public Animator anim;
    //Timer for length of anim
    float timer;
    //Starting timer for voice line
    bool startTimer;
    //bool to play once
    bool playonce;
    //How long animation lasts
    public float m_fHowLongToAnimate;



    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();

        startTimer = false;
        playonce = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        //Timer for length of voice line
        if (timer >= 20000.0f)
        {
            timer = 20000.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playonce)
            {
                //Starts Talking
                startTimer = true;
                anim.SetBool("Talking", true);
            }

            if (timer >= m_fHowLongToAnimate)
            {
                //Tavern keeper dead
                anim.SetBool("Dead", true);
                timer = 0;
                startTimer = false;
                playonce = false;
            }
        }
    }
}
