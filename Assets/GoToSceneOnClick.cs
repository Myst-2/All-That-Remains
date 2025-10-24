using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneOnClick : MonoBehaviour
{
    [SerializeField] string nextScene;   // e.g. "Page2", "Quote", "Game"

    public void OnClickNext()
    {
        Debug.Log($"[GoToSceneOnClick] Clicked → nextScene='{nextScene}'");
        if (SceneFader.Instance != null) SceneFader.LoadScene(nextScene);
        else SceneManager.LoadScene(nextScene);
    }
}
