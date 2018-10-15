using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEAnim_Joel : MonoBehaviour
{
    public GameObject Gerard;
    public GameObject QT;

    private float timer;

    Animator anim;

    public bool end;

    private QuickTimeEvent_Adrian qtScript;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        qtScript = Gerard.GetComponent<QuickTimeEvent_Adrian>();
        anim.SetBool("QTEnd", false);
        timer = 0.0f;
        end = false;
    }

    // Update is called once per frame
    void Update()
    {
        end = qtScript.getEnd();

        if(end)
        {
            timer += Time.deltaTime;
            anim.SetBool("QTEnd", true);

            if (timer >= 2.0f)
            {
                QT.SetActive(false);
                timer = 0.0f;
                qtScript.setEnd(false);
            }
        }
    }
}
