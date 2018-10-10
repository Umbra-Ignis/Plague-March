using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEAnim_Joel : MonoBehaviour
{
    public int timesPressed;
    public GameObject Gerard;
    public GameObject QT;

    private float timer;

    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("QTEnd", false);
        Gerard.SetActive(false);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Gerard.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            timesPressed++;
        }

        if(timesPressed >= 10)
        {
            timer += Time.deltaTime;
            anim.SetBool("QTEnd", true);

            if (timer >= 2.0f)
            {
                timesPressed = 0;
                Gerard.SetActive(true);
                QT.SetActive(false);
                timer = 0.0f;
            }
        }
    }
}
