using UnityEngine;
public class ItemMover : MonoBehaviour
{
    [HideInInspector]
    public float scrollSpeed = 5f;

    [HideInInspector]
    public Camera targetCamera;

    public bool isObstacle = false;

    public AudioClip collisionSound;

    public int bonusScore = 0;

    float GetCameraLeftX()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return float.NegativeInfinity;

        float distance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance)).x;
    }
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x < GetCameraLeftX())
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isObstacle)
            GameManager.Instance?.TakeDamage();
        else
            GameManager.Instance?.AddScore(bonusScore > 0 ? bonusScore : 1);

        if (collisionSound != null)
            AudioSource.PlayClipAtPoint(collisionSound, transform.position);

        Destroy(gameObject);
    }
}