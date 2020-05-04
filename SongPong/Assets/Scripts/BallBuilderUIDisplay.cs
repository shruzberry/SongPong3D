using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBuilderUIDisplay : MonoBehaviour
{
    public GameObject typeDisplay;
    public GameObject descriptionDisplay;
    public GameObject songController;
    public string[] ballTypes;
    public string[] ballDescriptions;

    private ClickTimer ct;
    private int ballType;
    private string typeIndex;

    // Start is called before the first frame update
    void Awake()
    {
        ct = songController.GetComponent<ClickTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        ballType = ct.ballTypeID;
        typeDisplay.GetComponent<Text>().text = ballTypes[ballType];
        descriptionDisplay.GetComponent<Text>().text = ballDescriptions[ballType];
    }
}
