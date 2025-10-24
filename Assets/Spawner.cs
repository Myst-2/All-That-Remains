using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float spawnCooldown = 1f;
    [SerializeField] private float randomOffset = 1f; // horizontal variation

    private float currentTime;

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = spawnCooldown;
            SpawnAttack();
        }
    }

    private void SpawnAttack()
    {
        // Base position from spawner in scene
        Vector3 spawnPos = transform.position;

        // Apply small random horizontal offset
        spawnPos.x += Random.Range(-randomOffset, randomOffset);

        // Spawn the prefab
        Instantiate(attackPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Spawned attack at {spawnPos}");
    }
}
 