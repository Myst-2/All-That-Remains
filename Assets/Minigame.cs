using UnityEngine;

public class Minigame : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float randomOffset = 1f;

    private void Start()
    {
        // Start from whatever the prefab’s base position is (e.g. -3.5, 10)
        Vector3 pos = transform.position;
        pos.x += Random.Range(-randomOffset, randomOffset); // Add a small offset from base
        transform.position = pos;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= -10f)
        {
            Destroy(gameObject);
        }
    }
}
