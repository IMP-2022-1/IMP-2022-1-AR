using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioClip Attacked;
    public AudioClip Spraying;
    public AudioClip KilledMosquito;
    public AudioClip MosquitoHeated;
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

    public void AttackedStart()
    {
        MyAudioSource.clip = Attacked;
        MyAudioSource.Play();
    }

    public void SprayingStart()
    {
        MyAudioSource.clip = Spraying;
        MyAudioSource.Play(); 
    }

    public void KilledMosquitoStart()
    {
        MyAudioSource.clip = KilledMosquito;
        MyAudioSource.Play();
    }
}
