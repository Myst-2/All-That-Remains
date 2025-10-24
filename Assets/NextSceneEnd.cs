using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class NextSceneEnd : MonoBehaviour
{
    [Header("Load")]
    [SerializeField] string nextSceneName = "DestroyedHouse";
    [SerializeField] bool useFader = true;

    [Header("Optional Pose Lock")]
    [SerializeField] PlayableDirector director;  // drag the same PlayableDirector here
    [SerializeField] bool lockPose = true;

    public void LoadNext()
    {
        // Freeze on the very last frame so there’s no “snap” or restart.
        if (lockPose && director)
        {
            director.time = director.duration;
            director.Evaluate();

            var anims = director.GetComponentsInChildren<Animator>(true);
            foreach (var a in anims)
            {
#if UNITY_2020_1_OR_NEWER
                a.keepAnimatorStateOnDisable = true;
#endif
                a.speed = 0f;
                a.enabled = false;
            }
        }

        if (useFader && typeof(SceneFader) != null) SceneFader.LoadScene(nextSceneName);
        else SceneManager.LoadScene(nextSceneName);
    }
}
