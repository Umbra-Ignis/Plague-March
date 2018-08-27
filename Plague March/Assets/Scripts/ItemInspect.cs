using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInspect : MonoBehaviour
{

    public Text tooltipText;
    public Image inspectTarget;

    private bool inTrigger;

    // Use this for initialization
    void Start()
    {
        tooltipText.enabled = false;
        inspectTarget.enabled = false;
        inTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTrigger)
        {
            inspectTarget.enabled = true;
            tooltipText.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inspectTarget.enabled = false;
            if (inTrigger)
                tooltipText.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            tooltipText.enabled = true;
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            tooltipText.enabled = false;
            inTrigger = false;
        }
    }
}
