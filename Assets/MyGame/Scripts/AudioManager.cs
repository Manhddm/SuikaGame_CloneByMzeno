using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [Header("SFX Clips")]
    public AudioClip mergeSfx;
    public AudioClip dropSfx;
    public AudioClip gameOverSfx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void PlayMergeSfx()
    {
        PlaySfx(mergeSfx);
    }

    public void MuteMusic(bool value)
    {
        musicSource.mute = value;
    }
    public void PlayDropSfx()
    {
        PlaySfx(dropSfx);
    }

    public void PlayGameOverSfx()
    {
        PlaySfx(gameOverSfx);
    }
    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
