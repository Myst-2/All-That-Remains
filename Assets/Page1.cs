using UnityEngine;

public class Page1 : MonoBehaviour
{
    [SerializeField] string nextScene = "Page2";
    public void OnClickNext() => SceneFader.LoadScene(nextScene);
}
