using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneOnClickP2 : MonoBehaviour
{
    [SerializeField] string nextScene;   // e.g. "Page2", "Quote", "Game"

    public void OnClickNext()
    {
        // Use the global fader if it exists, otherwise fall back to direct load
        if (SceneFader.Instance != null) SceneFader.LoadScene(nextScene);
        else SceneManager.LoadScene(nextScene);
    }
}
