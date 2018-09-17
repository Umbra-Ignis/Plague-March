using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInspect : MonoBehaviour
{

    public Text tooltipText = null;
    public Image inspectTarget = null;

    private bool inTrigger;

    // Use this for initialization
    void Start()
    {
        inTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTrigger)
        {
            if (inspectTarget != null)
                inspectTarget.enabled = true;

            if (tooltipText != null)
                tooltipText.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inspectTarget != null)
                inspectTarget.enabled = false;


            if (inTrigger && tooltipText != null)
                tooltipText.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            if (tooltipText != null)
                tooltipText.enabled = true;
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            if (tooltipText != null)
                tooltipText.enabled = false;
            inTrigger = false;
        }
    }
}
