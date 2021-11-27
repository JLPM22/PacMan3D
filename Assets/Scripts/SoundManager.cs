using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip Siren;
    public AudioClip PacManDeath;
    public AudioClip PacManEat;
    public AudioClip PacManEatGhost;
    public AudioClip SirenEatingGhost;

    private AudioSource AudioSource;

    private void Awake()
    {
        Debug.Assert(Instance == null, "There can only be one SoundManager");
        Instance = this;

        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySiren();
    }

    public void PlaySiren()
    {
        if (AudioSource.clip != Siren)
        {
            AudioSource.Stop();
            AudioSource.clip = Siren;
            AudioSource.Play();
        }
    }

    public void PlayPacManEat()
    {
        if (AudioSource.clip != PacManEat)
        {
            AudioSource.Stop();
            AudioSource.clip = PacManEat;
            AudioSource.Play();
        }
    }

    public void PlayEatingGhost()
    {
        if (AudioSource.clip != SirenEatingGhost)
        {
            AudioSource.Stop();
            AudioSource.clip = SirenEatingGhost;
            AudioSource.Play();
        }
    }

    public void PlayPacManDeath()
    {
        AudioSource.PlayOneShot(PacManDeath);
    }

    public void PlayPacManEatGhost()
    {
        AudioSource.PlayOneShot(PacManEatGhost);
    }
}
