using UnityEngine;

public class TestControl : MonoBehaviour
{

    Rigidbody2D rb; 


    [Header("Movement Settings")]
    public float moveSpeed = 3f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        //---Movement---//
        if (Input.GetKey(KeyCode.W))
        {
            direction += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += new Vector3(-1, 0, 0);
        }

        
        direction = direction.normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
