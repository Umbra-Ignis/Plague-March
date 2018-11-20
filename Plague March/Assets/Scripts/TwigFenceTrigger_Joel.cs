//========================================================================================
//TwigFenceTrigger
//
//Functionality: Manages the standing up and laying down
//
//Author: Joel G
//========================================================================================
using UnityEngine;

public class TwigFenceTrigger_Joel : MonoBehaviour
{
    public GameObject fenceStand;
    public GameObject fenceLay;
    public bool stand2Lay;
    public bool lay2Stand;

    private bool doneOnce;

    // Use this for initialization
    void Start()
    {
        if(stand2Lay)
        {
            fenceStand.SetActive(true);
            fenceLay.SetActive(false);
        }

        if(lay2Stand)
        {
            fenceStand.SetActive(false);
            fenceLay.SetActive(true);
        }

        doneOnce = false;
    }

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter(Collider other)
    {
        if (!doneOnce)
        {
            if (other.CompareTag("Player") && stand2Lay)
            {
                fenceStand.SetActive(false);
                fenceLay.SetActive(true);
            }

            if (other.CompareTag("Player") && lay2Stand)
            {
                fenceStand.SetActive(true);
                fenceLay.SetActive(false);
            }

            doneOnce = true;
        }
    }
}
