using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // Player
    public Vector2 followThreshold = new Vector2(0f, 2f); // How far the player can move up before camera follows
    public float cameraSpeed = 5f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 cameraPos = transform.position;

        float verticalDist = target.position.y - transform.position.y;

        // If frog goes above or below threshold, camera follows
        if (Mathf.Abs(verticalDist) > followThreshold.y)
        {
            float targetY = target.position.y - Mathf.Sign(verticalDist) * followThreshold.y;
            cameraPos.y = Mathf.Lerp(cameraPos.y, targetY, Time.deltaTime * cameraSpeed);
        }

        // Optional: Add horizontal threshold too if needed

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
    }
}
