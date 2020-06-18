using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActiveSongData : MonoBehaviour
{
    public SongData editingSong;
    public bool syncWithRuntime = false;
    private BallDropper ballDropper;
    private LevelChanger levelChanger;

    void OnEnable()
    {
        ballDropper = FindObjectOfType<BallDropper>();
        levelChanger = FindObjectOfType<LevelChanger>();
    }
    
    void Update()
    {
        if(Application.isEditor)
        {
            ballDropper.ballMapName = editingSong.name;
        }
        
        if(syncWithRuntime)
        {
            editingSong = levelChanger.songData;
        }
    }
}
