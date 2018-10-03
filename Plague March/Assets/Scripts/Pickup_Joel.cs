using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[HideInInspector]
public enum pickupType
{
    key1,
    key2,
    key3,
    key4,
    note1,
    note2,
    note3,
    note4,
    note5
}

public class Pickup_Joel : MonoBehaviour
{
    public pickupType type;
    public Text tooltip;
    public Canvas TooltipCanvas;
    private UIController_Joel ui;
    private bool toBePickedUp;

    // Use this for initialization
    void Start()
    {
        tooltip.enabled = false;
        ui = TooltipCanvas.GetComponent<UIController_Joel>();
        toBePickedUp = true;
    }

    // Update is called once per frame
    void Update() {}

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && toBePickedUp)
        {
            tooltip.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                ui.TurnOnItem((int)type);
                tooltip.enabled = false;
                toBePickedUp = false;
                other.GetComponent<UserControler_Adrian>().obtainedKey((int)type + 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tooltip.enabled = false;
        }
    }
}
