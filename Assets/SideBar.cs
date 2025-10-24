using UnityEngine;

public class SideBar : MonoBehaviour
{
    Vector2 velocity;
    float lifetime;
    int damage;
    Health playerHP;
    BoxCollider2D safeZone;
    Rect arena;

    public void Init(Vector2 vel, float life, int dmg, Health hp, BoxCollider2D safe, Rect a)
    {
        velocity = vel;
        lifetime = life;
        damage = dmg;
        playerHP = hp;
        safeZone = safe;
        arena = a;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Destroy if outside arena bounds
        if (!arena.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Check if inside safe zone
        if (safeZone && safeZone.bounds.Contains(other.transform.position))
        {
            Debug.Log("Player is safe!");
            return;
        }

        // Otherwise deal damage
        if (playerHP)
        {
            playerHP.TakeDamage(damage);
            Debug.Log("Player hit! -" + damage + " HP");
        }

        Destroy(gameObject);
    }
}
