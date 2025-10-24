using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public float stepTime = 0.2f;
    public LayerMask solidMask;

    private bool isMoving = false;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private Animator _animator;

    
    [Header("Audio")]
    [SerializeField] private FootstepSFX footstep;

    void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (_animator == null) _animator = GetComponent<Animator>();
        if (!footstep) footstep = GetComponent<FootstepSFX>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
            return;
        }

        if (isMoving) return;

        Vector3 dir = Vector3.zero;
        if      (Input.GetKey(KeyCode.W)) dir = Vector3.up;
        else if (Input.GetKey(KeyCode.S)) dir = Vector3.down;
        else if (Input.GetKey(KeyCode.A)) dir = Vector3.left;
        else if (Input.GetKey(KeyCode.D)) dir = Vector3.right;

        if (dir != Vector3.zero)
        {
            SetDirectionBools(dir);
            StartCoroutine(MoveStep(dir));
        }
        else
        {
            SetIdleBools();
        }
    }

    IEnumerator MoveStep(Vector3 dir)
    {
        isMoving = true;

        Vector2 start = rb.position;
        Vector2 end   = start + (Vector2)dir;

        // destination collision check
        Vector2 boxSize = col.bounds.size * 0.95f;
        bool blocked = Physics2D.OverlapBox(end, boxSize, 0f, solidMask) != null;
        if (blocked) { isMoving = false; yield break; }

        // STEP SFX: initial footfall
        footstep?.Step();

        float t = 0f;
        bool midPlayed = false;
        while (t < 1f)
        {
            t += Time.deltaTime / stepTime;

            // optional mid-step footfall (feels nice with longer steps)
            if (!midPlayed && t >= 0.5f)
            {
                footstep?.Step();
                midPlayed = true;
            }

            Vector2 target = Vector2.Lerp(start, end, t);
            rb.MovePosition(target);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(end);
        isMoving = false;

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            SetIdleBools();
        }
    }

    void SetDirectionBools(Vector3 dir)
    {
        _animator.SetBool("isBack",    dir == Vector3.up);
        _animator.SetBool("isForward", dir == Vector3.down);
        _animator.SetBool("isLeft",    dir == Vector3.left);
        _animator.SetBool("isRight",   dir == Vector3.right);
    }

    void SetIdleBools()
    {
        _animator.SetBool("isForward", false);
        _animator.SetBool("isBack",    false);
        _animator.SetBool("isLeft",    false);
        _animator.SetBool("isRight",   false);
    }

    void OnDrawGizmosSelected()
    {
        if (col == null) return;
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        var size = col.bounds.size * 0.95f;
        Gizmos.DrawCube((Vector2)transform.position, size);
    }
}
