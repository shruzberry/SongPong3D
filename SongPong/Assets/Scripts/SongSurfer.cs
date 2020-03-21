using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSurfer : MonoBehaviour
{
    private AudioSource song; //The Song that will be located and changed
    private float songTime = 0.0f; //Time to change the song to on slider inpu

    void Awake()
    {
        song = GetComponent<AudioSource>();
    }

    public void setTime(float time)
    {
        song.time = (song.clip.length * time);
        song.Play();
    }
}
