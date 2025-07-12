using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolumes();
        if (menuMusic) PlayMusic(menuMusic);
    }

    // Public Functions
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    public void PlaySound(string name)
    {
        // Shortcut mapping for convenience
        switch (name.ToLower())
        {
            case "click": PlaySound(clickSound); break;
            case "correct": PlaySound(correctSound); break;
            case "incorrect": PlaySound(incorrectSound); break;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeTime = 1f)
    {
        StartCoroutine(FadeInMusic(clip, fadeTime));
    }

    public void StopMusic(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutMusic(fadeTime));
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    private void ApplyVolumes()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    // Coroutines for fade effects
    private IEnumerator FadeInMusic(AudioClip clip, float duration)
    {
        if (musicSource.isPlaying) yield return FadeOutMusic(duration / 2f);

        musicSource.clip = clip;
        musicSource.Play();
        musicSource.volume = 0f;

        float timer = 0f;
        while (timer < duration)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = musicVolume;
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = musicVolume;
    }
}
