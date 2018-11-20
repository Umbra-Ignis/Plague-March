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

    public Animator anim;
    float timer;
    bool startTimer;
    bool playonce;
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
        Debug.Log(anim.GetFloat("Blend"));
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
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
                startTimer = true;
                anim.SetBool("Talking", true);
            }

            if (timer >= m_fHowLongToAnimate)
            {
                anim.SetBool("Dead", true);
                timer = 0;
                startTimer = false;
                playonce = false;
            }
        }
    }
}
