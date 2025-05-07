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
            SpawnEnemy(); // Call the method to spawn enemies
            spawnTimer = 0f; // Reset the spawn timer
            currentEnemyCount++; // Increment the current enemy count
        }
    }

    void SpawnEnemy()
    {
        // Instantiate the enemy prefab at the spawner's position and rotation
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        
        // Set the parent of the spawned enemy to this spawner object
        enemy.transform.parent = transform;
    }
}
