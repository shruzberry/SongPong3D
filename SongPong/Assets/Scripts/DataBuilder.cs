using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataBuilder : MonoBehaviour
{
    public string fileName;
    public float bpm;

    private string path;
    private StreamWriter writer;

    void Awake()
    {
        path = "Assets/Resources/SongData/" + fileName + ".csv";
        writer = new StreamWriter(path, true);
    }

    void AddPlaceholder()
    {
        writer.WriteLine("null, ");
    }
}
