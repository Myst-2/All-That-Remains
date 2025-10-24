using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackDamage : MonoBehaviour
{
    [SerializeField] int damage = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Attack hit {other.name}");

        var hp = other.GetComponent<Health>();
        if (hp != null)
        {
            Debug.Log($"Dealt {damage} damage to {other.name}");
            hp.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
