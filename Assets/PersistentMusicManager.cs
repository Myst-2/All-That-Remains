using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PersistentMusicManager : MonoBehaviour
{
    public static PersistentMusicManager Instance;

    [Header("Behavior when a scene has NO MusicTag")]
    [Tooltip("If true, we fade out and stop when a scene has no MusicTag. If false, we keep playing the current music.")]
    public bool stopIfNoTag = false;

    [Header("Defaults for the very first scene (optional)")]
    public AudioClip initialClip;
    [Range(0f, 1f)] public float initialVolume = 0.8f;
    public bool initialLoop = true;
    public float initialFadeIn = 1f;

    AudioSource src;
    Coroutine xfadeCo;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = true;
        src.volume = 0f;

        SceneManager.sceneLoaded += OnSceneLoaded;

        // If you want music immediately in the first scene, set initialClip in the inspector
        if (initialClip)
        {
            src.clip = initialClip;
            src.loop = initialLoop;
            src.Play();
            if (initialFadeIn > 0f) StartCoroutine(FadeTo(initialVolume, initialFadeIn));
            else src.volume = initialVolume;
        }

        // Process the first scene's tag (if any)
        ApplySceneTagNow();
    }

    void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        ApplySceneTagNow();
    }

    void ApplySceneTagNow()
    {
        var tag = FindObjectOfType<MusicTag>();
        if (!tag)
        {
            if (stopIfNoTag) { StartFadeOutAndStop(); }
            // else keep playing whatever was already going
            return;
        }

        // If same clip is already playing
        if (src.clip == tag.music && src.isPlaying && !tag.restartIfSame)
        {
            // Just adjust volume/loop smoothly
            if (xfadeCo != null) StopCoroutine(xfadeCo);
            xfadeCo = StartCoroutine(FadeTo(tag.volume, tag.fadeIn > 0 ? tag.fadeIn : 0.1f));
            src.loop = tag.loop;
            return;
        }

        // Otherwise crossfade to the new clip
        StartCrossfade(tag.music, tag.volume, tag.loop, tag.fadeOut, tag.fadeIn);
    }

    void StartCrossfade(AudioClip next, float targetVol, bool loop, float outSec, float inSec)
    {
        if (xfadeCo != null) StopCoroutine(xfadeCo);
        xfadeCo = StartCoroutine(Crossfade(next, targetVol, loop, outSec, inSec));
    }

    IEnumerator Crossfade(AudioClip next, float targetVol, bool loop, float outSec, float inSec)
    {
        float fromVol = src.volume;
        float t = 0f;

        // Fade out current
        while (t < outSec)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(fromVol, 0f, outSec <= 0 ? 1f : t / outSec);
            yield return null;
        }
        src.volume = 0f;

        // Swap & fade in
        src.clip = next;
        src.loop = loop;
        if (next) src.Play();

        t = 0f;
        while (t < inSec)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, targetVol, inSec <= 0 ? 1f : t / inSec);
            yield return null;
        }
        src.volume = targetVol;
        xfadeCo = null;
    }

    void StartFadeOutAndStop()
    {
        if (xfadeCo != null) StopCoroutine(xfadeCo);
        xfadeCo = StartCoroutine(FadeOutAndStop(0.5f));
    }

    IEnumerator FadeOutAndStop(float seconds)
    {
        float start = src.volume;
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0f, seconds <= 0 ? 1f : t / seconds);
            yield return null;
        }
        src.Stop();
        xfadeCo = null;
    }

    IEnumerator FadeTo(float target, float seconds)
    {
        float start = src.volume;
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, target, seconds <= 0 ? 1f : t / seconds);
            yield return null;
        }
        src.volume = target;
    }
}
