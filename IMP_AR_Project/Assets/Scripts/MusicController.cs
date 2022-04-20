using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip MainMusic;
    public AudioClip PlaySound;
    public AudioClip GameOverMusic;
    private AudioSource MyAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMusicStart ()
    {
        if (MyAudioSource.isPlaying == false)
        {
            MyAudioSource.clip = MainMusic;
            MyAudioSource.Play();
        }
    }

    public void PlaySoundStart ()
    {
        MyAudioSource.clip = PlaySound;
        MyAudioSource.Play(); // restart Play implement require
    }

    public void GameOverMusicStart ()
    {
        MyAudioSource.clip = GameOverMusic;
        MyAudioSource.Play();
    }
}
