using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer instance;
    public AudioClip[] audioSources;
    AudioSource audio;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { Destroy(this.gameObject); }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
