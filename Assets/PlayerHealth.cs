using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] string sceneOnDeath = "Death";
    void Start() => onDeath += () => SceneManager.LoadScene(sceneOnDeath);
}
