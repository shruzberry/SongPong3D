using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class beat : MonoBehaviour
{
    private TextMeshProUGUI beatNum;
    private SongController sc;

    // Start is called before the first frame update
    void Start()
    {
        beatNum = GetComponent<TextMeshProUGUI>();
        sc = GameObject.Find("SongController").GetComponent<SongController>();
    }

    // Update is called once per frame
    void Update()
    {
        beatNum.text = "" + Mathf.Round(sc.GetSongTimeBeats());
    }
}
