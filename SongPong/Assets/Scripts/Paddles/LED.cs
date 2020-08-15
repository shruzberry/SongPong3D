using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour
{
    // COMPONENTS
    private Material material;

    // REFERNECES
    private SongController sc;

    // PUBLIC VARS
    public int order; // what number in the order this LED is

    // PRIVATE VARS
    private bool isOn;
    private float beatsPerLoop;

    public void Initialize(PaddleManager pm)
    {
        material = GetComponent<MeshRenderer>().material;
        sc = FindObjectOfType<SongController>();
        TurnLEDOff();
        beatsPerLoop = pm.beatsPerFlash;
        float toggleDelay = (float)order / 10f;
        float toggleRate = sc.ToTime(beatsPerLoop);

        InvokeRepeating("ToggleLED", 2.0f - toggleDelay, toggleRate);
        InvokeRepeating("ToggleLEDOff", 2.0f - toggleDelay + 0.2f, toggleRate);
    }

    public void ToggleLED()
    {
        TurnLEDOn();
    }

    public void ToggleLEDOff()
    {
        TurnLEDOff();
    }

    public void TurnLEDOn()
    {
        isOn = true;
        material.EnableKeyword("_EMISSION");
    }

    public void TurnLEDOff()
    {
        isOn = false;
        material.DisableKeyword("_EMISSION");
    }
}
