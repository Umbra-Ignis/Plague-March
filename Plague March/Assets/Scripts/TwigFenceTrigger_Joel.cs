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
    //Fence objects
    public GameObject fenceStand;
    public GameObject fenceLay;

    [Space]

    //Fence positions
    public bool stand2Lay;
    public bool lay2Stand;

    private bool doneOnce;

    // Use this for initialization
    void Start()
    {
        //Change frence to closed
        if(stand2Lay)
        {
            fenceStand.SetActive(true);
            fenceLay.SetActive(false);
        }
        //Change fence to open
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
            //Used on trigger
            //Fence closed
            if (other.CompareTag("Player") && stand2Lay)
            {
                fenceStand.SetActive(false);
                fenceLay.SetActive(true);
            }
            //Fence open
            if (other.CompareTag("Player") && lay2Stand)
            {
                fenceStand.SetActive(true);
                fenceLay.SetActive(false);
            }

            doneOnce = true;
        }
    }
}
