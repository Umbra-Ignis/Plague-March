//========================================================================================
//TooltipManager
//
//Functionality: Used to manage the popping up and down of certain tooltips
//
//Author: Joel G
//========================================================================================
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager_Joel : MonoBehaviour
{
    public Image tooltipImage;

    // Use this for initialization
    void Start()
    {
        tooltipImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            tooltipImage.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tooltipImage.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tooltipImage.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tooltipImage.enabled = false;
        }
    }
}
