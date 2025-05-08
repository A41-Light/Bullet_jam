using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public Transform[] spawnPoint; // Point where the enemy will spawn
    public float spawnInterval = 2.0f; // Time interval between spawns in seconds
    public int maxEnemies = 5; // Maximum number of enemies allowed in the scene

    private int currentEnemyCount = 0; // Current number of enemies in the scene
    private float spawnTimer =0f; // Timer to track spawn intervals

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime; // Increment the spawn timer by the time since the last frame

        if(spawnTimer >= spawnInterval && currentEnemyCount < maxEnemies)
        {
            SpawnEnemyOutsideScreen(); // Call the method to spawn enemies
            spawnTimer = 0f; // Reset the spawn timer
            currentEnemyCount++; // Increment the current enemy count
        }
    }

    void SpawnEnemyOutsideScreen()
    {
    Camera cam = Camera.main;
    float height = 2f * cam.orthographicSize;
    float width = height * cam.aspect;

    float left = cam.transform.position.x - width / 2f;
    float right = cam.transform.position.x + width / 2f;
    float top = cam.transform.position.y + height / 2f;
    float bottom = cam.transform.position.y - height / 2f;

    Vector2 spawnPos = Vector2.zero;
    int side = Random.Range(0, 4);

    switch (side)
    {
        case 0: spawnPos = new Vector2(Random.Range(left, right), top + 1f); break;
        case 1: spawnPos = new Vector2(Random.Range(left, right), bottom - 1f); break;
        case 2: spawnPos = new Vector2(left - 1f, Random.Range(bottom, top)); break;
        case 3: spawnPos = new Vector2(right + 1f, Random.Range(bottom, top)); break;
    }

    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
}

}
