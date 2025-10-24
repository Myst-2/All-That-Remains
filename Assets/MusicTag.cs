using UnityEngine;

public class MusicTag : MonoBehaviour
{
    [Header("What should play in this scene")]
    public AudioClip music;
    [Range(0f, 1f)] public float volume = 0.8f;
    public bool loop = true;

    [Header("Fades")]
    public float fadeIn = 1f;
    public float fadeOut = 0.5f;

    [Header("If the same clip is already playing…")]
    public bool restartIfSame = false;   // usually FALSE for Battle <-> TestMinigame
}
