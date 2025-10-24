using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : Health
{
    [SerializeField] string sceneOnDeath = "switchout";
    void Start() => onDeath += () => SceneManager.LoadScene(sceneOnDeath);
}
