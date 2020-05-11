using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongReader : MonoBehaviour
{
    //___________Reader__________________
    public string dataLocation = "Assets/Resources/Song Data/data";
    public string songMapName;
    public string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = dataLocation + songMapName;
        print("DATA PATH: " + dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ARCHIVED
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /*
    public void loadBalls()
    {
        //print("loading");
        song.loadSong(); // this should not be here

        // Load the Ball Sheet and the Notes Sheet
        string path = ballsLocation + ballMapName + "/";
        //print(path);
        //print(ballsLocation);
        //print(ballMapName);
        string ballsPath = path + ballMapName + "_balls.csv";
        //print(ballsPath);

        StreamReader reader = new StreamReader(ballsPath);
        string currLine = reader.ReadLine(); // this skips the labels row

        while((currLine = reader.ReadLine()) != null){
            // Get info from sheet
            string[] ballInfo = currLine.Split(delimeter);
            int id = int.Parse(ballInfo[idColumn]);
            string type = ballInfo[typeColumn].ToLower();

            // Get the balls' notes from Song
            string[] noteString = ballInfo[notesColumn].Split(noteDelimeter);
            int[] notes = Array.ConvertAll<string, int>(noteString, int.Parse);
            List<Note> noteList = song.getNotes(notes);

            // Spawn the Ball depending on the type
            switch(type){
                case "basic":
                    spawnSimpleBall(id,noteList);
                    break;
                case "bounce":
                    spawnBounceBall(id,noteList);
                    break;
                default:
                    break;
            }
        }
    }
    */
}
