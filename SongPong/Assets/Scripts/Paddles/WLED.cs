using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WLED : MonoBehaviour
{
    private Material material;

    private bool isOn;

    public void Initialize(PaddleMover pm)
    {
        material = GetComponent<MeshRenderer>().material;
        TurnLEDOn();
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
