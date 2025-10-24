using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Drag the Player here in Inspector
    public float smoothSpeed = 5f; // How fast the camera catches up

    void LateUpdate()
    {
        if (target == null) return;

        // Follow player’s X and Y, but keep camera’s Z
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}
