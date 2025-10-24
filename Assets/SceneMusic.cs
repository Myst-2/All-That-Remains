// SceneMusic.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SceneMusic : MonoBehaviour
{
    [Header("Track")]
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.6f;
    public bool loop = true;

    [Header("Fade")]
    public float fadeInSeconds = 1f;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = loop;
        src.clip = clip;
        src.volume = 0f; // start silent, then fade in
    }

    void Start()
    {
        if (clip == null) return;
        src.Play();
        if (fadeInSeconds > 0f) StartCoroutine(FadeIn());
        else src.volume = volume;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeInSeconds)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, volume, t / fadeInSeconds);
            yield return null;
        }
        src.volume = volume;
    }
}
