using UnityEngine;

public class BattlePortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        SceneFader.LoadScene("Battle");  // smooth transition
    }
}
