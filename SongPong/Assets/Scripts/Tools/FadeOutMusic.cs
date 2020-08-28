using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMusic : MonoBehaviour
{
    public float rate = 1.0f;

    private AudioSource _source;
    private SongController _sc;
    private bool _fadeOut = false;

    // Start is called before the first frame update
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _sc = GetComponent<SongController>();
    }

    void OnEnable()
    {
        //_sc.onSongEnd += BeginFadeOut;
    }

    void OnDisable()
    {
        //_sc.onSongEnd -= BeginFadeOut;
    }

    void BeginFadeOut()
    {
        _fadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_fadeOut)
        {
            _source.volume -= rate * Time.deltaTime;
        }
    }
}
