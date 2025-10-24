using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] Health playerHP;
    [SerializeField] int damagePerHit = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
            playerHP.TakeDamage(damagePerHit);
    }
}
