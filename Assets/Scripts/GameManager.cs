using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    float itemSpawnInterval = 2f;

    [SerializeField]
    Vector2 itemSpawnYRange = new Vector2(-2f, 2f);

    [SerializeField]
    Vector2 itemSpawnXRange = new Vector2(-2f, 2f);

    [SerializeField]
    Transform itemParent;

    [SerializeField] private Camera targetCamera;

    [SerializeField] private float scrollSpeed = 10f;

    [SerializeField]
    TextMeshProUGUI scoreText;

    float itemSpawnTimer;
    int itemSpawnCount;

    [SerializeField]
    int maxHP = 3;
    int hp;
    bool isGameOver = false;

    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField] private int itemsPerObstacle = 5;


    public static GameManager Instance { get; private set; }

    int score = 0;
    public int Score => score;

    public void AddScore(int amount = 1)
    {
        score += amount;
        Debug.Log($"Score: {score}");
        scoreText.text = $"Score: {score}";
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isGameOver) return;

        itemSpawnTimer += Time.deltaTime;

        if (itemSpawnTimer < itemSpawnInterval)
            return;

        itemSpawnTimer = 0f;

        itemSpawnCount++;
        if (itemSpawnCount >= itemsPerObstacle)
        {
            itemSpawnCount = 0;
            SpawnObstacle();
        }
        else
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {

        float spawnX = GetCameraRightX();
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, itemParent);

        ItemMover mover = item.GetComponent<ItemMover>();
        if (mover == null)
            mover = item.AddComponent<ItemMover>();

        mover.scrollSpeed = scrollSpeed;
        mover.targetCamera = targetCamera != null ? targetCamera : Camera.main;
    }

    void SpawnObstacle()
    {
        float spawnX = GetCameraRightX();
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, itemParent);

        ItemMover mover = obstacle.GetComponent<ItemMover>();
        if (mover == null)
            mover = obstacle.AddComponent<ItemMover>();

        mover.scrollSpeed = scrollSpeed;
        mover.targetCamera = targetCamera != null ? targetCamera : Camera.main;
        mover.isObstacle = true;
    }

    float GetCameraRightX()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return 10f;

        float distance = Mathf.Abs(cam.transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, distance)).x;
    }
    void Start()
    {
        hp = maxHP;
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void TakeDamage()
    {
        if (isGameOver) return;

        hp--;
        Debug.Log($"HP: {hp}");

        if (hp <= 0)
            GameOver();
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over");
    }
}
