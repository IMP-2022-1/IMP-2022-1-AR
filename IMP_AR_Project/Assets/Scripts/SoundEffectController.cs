using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioClip Attacked;
    public AudioClip Spraying;
    public AudioClip KilledMosquito;
    public AudioClip MosquitoHeated;
    public AudioClip ReadyToPlaySigh;
    public AudioClip ReadyToPlayAlarm;
    public AudioClip ReadyToPlayBeep;
    public AudioClip FirstAid;
    private AudioSource MyAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
    }

    public void AttackedStart()
    {
        MyAudioSource.clip = Attacked;
        MyAudioSource.Play();
    }

    public void SprayingStart()
    {
        MyAudioSource.Stop();
        MyAudioSource.clip = Spraying;
        MyAudioSource.Play();
    }

    public void KilledMosquitoStart()
    {
        MyAudioSource.clip = KilledMosquito;
        MyAudioSource.Play();
    }

    public void ReadyToPlay_Sigh()
    {
        MyAudioSource.clip = ReadyToPlaySigh;
        MyAudioSource.Play();
    }

    public void ReadyToPlay_Alarm()
    {
        MyAudioSource.clip = ReadyToPlayAlarm;
        MyAudioSource.Play();
    }

    public void ReadyToPlay_Beep()
    {
        MyAudioSource.clip = ReadyToPlayBeep;
        MyAudioSource.Play();
    }

    public void Acquire_First_Aid()
    {
        MyAudioSource.clip = FirstAid;
        MyAudioSource.Play();

        /*// Acquire FirstAid Sound Start
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().Acquire_First_Aid();
        */
    }
}
