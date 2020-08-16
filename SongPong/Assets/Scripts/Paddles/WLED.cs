using UnityEngine;

public class WLED : MonoBehaviour
{
    private Material material;

    public void Initialize(Paddle pm)
    {
        material = GetComponent<MeshRenderer>().material;
        TurnLEDOn();
    }

    public void TurnLEDOn()
    {
        material.EnableKeyword("_EMISSION");
    }

    public void TurnLEDOff()
    {
        material.DisableKeyword("_EMISSION");
    }
}
