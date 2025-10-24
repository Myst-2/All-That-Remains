// AttackMover.cs
using UnityEngine;

public class AttackMover : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float selfDestructY = -20f;

    public void SetSpeed(float s) => speed = s;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y < selfDestructY) Destroy(gameObject);
    }
}
