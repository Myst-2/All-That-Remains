// MainMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearBattle : MonoBehaviour
{
    [SerializeField] string gameSceneName = "Game";

    public void StartGame()
    {
        BattleData.Clear();               // <-- reset all saved HP
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
