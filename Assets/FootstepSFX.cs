using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepSFX : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;               // 1+ soft generic steps
    [SerializeField] Vector2 pitchJitter = new(0.96f, 1.04f);
    [SerializeField] float volume = 0.8f;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
    }

    public void Step()
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        src.pitch = Random.Range(pitchJitter.x, pitchJitter.y);
        src.PlayOneShot(clip, volume);
    }
}
