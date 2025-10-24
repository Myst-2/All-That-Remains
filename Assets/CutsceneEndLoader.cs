using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneEndLoader : MonoBehaviour
{
    [SerializeField] string nextSceneName = "DestroyedHouse";
    [SerializeField] bool useFader = false;       // uncheck while testing
    [SerializeField] float postHoldSeconds = 0.05f; // tiny pause after last frame

    PlayableDirector director;
    Animator[] animators;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();

        // Hold last frame when the timeline ends
        director.extrapolationMode = DirectorWrapMode.Hold;

        // Cache any animators so we can lock their pose at the end
        animators = GetComponentsInChildren<Animator>(true);
    }

    void Start()
    {
        if (director.playableAsset == null)
        {
            Debug.LogError("[CutsceneEndLoader] No Timeline assigned to the Playable Director.");
            return;
        }

        // Ensure we start from 0 and actually play
        director.time = 0;
        director.Play();

        StartCoroutine(WaitThenLoad());
    }

    IEnumerator WaitThenLoad()
    {
        // Wait while timeline is playing
        while (director.state == PlayState.Playing)
            yield return null;

        // Force the very last frame to be evaluated (prevents flicker/reset)
        director.time = director.duration;
        director.Evaluate();

        // Lock all animator poses so they won’t snap back
        foreach (var a in animators)
        {
            if (!a) continue;
#if UNITY_2020_1_OR_NEWER
            a.keepAnimatorStateOnDisable = true;
#endif
            a.speed = 0f;     // freeze time
            a.enabled = false;
        }

        // Briefly hold the frozen pose (helps hides under fades)
        if (postHoldSeconds > 0f)
            yield return new WaitForSecondsRealtime(postHoldSeconds);

        // Load the next scene
        if (useFader) SceneFader.LoadScene(nextSceneName);
        else SceneManager.LoadScene(nextSceneName);
    }
}
