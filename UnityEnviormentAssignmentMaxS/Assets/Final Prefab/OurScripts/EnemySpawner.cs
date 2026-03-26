using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;          // Drag your enemy prefabs here
    public int maxEnemiesAtOnce = 10;          // Won't spawn more than this at a time
    public float minSpawnInterval = 5f;        // Min seconds between spawns
    public float maxSpawnInterval = 15f;       // Max seconds between spawns

    [Header("Spawn Area")]
    public float spawnRadius = 30f;
    public float navMeshSampleDistance = 5f;
    public float minSpawnDistanceFromPlayer = 10f; // Won't spawn too close to the player

    [Header("References")]
    public Transform spawnCenter;              // Leave empty to use this object's position
    public Transform player;                   // Assign the player so enemies don't spawn on top of them

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (spawnCenter == null)
            spawnCenter = transform;

        // Auto-find player by tag if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        else
            StartCoroutine(TrickleLoop());
    }

    // Trickle mode: spawns one enemy at a time on a random interval
    IEnumerator TrickleLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            activeEnemies.RemoveAll(e => e == null); // Clean up dead enemies

            if (activeEnemies.Count < maxEnemiesAtOnce)
                TrySpawn();
        }
    }

    void TrySpawn()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemy prefabs assigned!");
            return;
        }

        // Try a few times to find a valid point that's far enough from the player
        int attempts = 10;
        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0f;
            Vector3 candidatePoint = spawnCenter.position + randomOffset;

            // Reject points too close to the player
            if (player != null && Vector3.Distance(candidatePoint, player.position) < minSpawnDistanceFromPlayer)
                continue;

            if (NavMesh.SamplePosition(candidatePoint, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                SpawnEnemy(hit.position);
                return;
            }
        }

        Debug.Log("EnemySpawner: Couldn't find a valid spawn point after max attempts, skipping.");
    }

    void SpawnEnemy(Vector3 position)
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject spawned = Instantiate(prefab, position, Quaternion.identity);
        activeEnemies.Add(spawned);

        Debug.Log($"Spawned {spawned.name} at {position}");
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = spawnCenter != null ? spawnCenter.position : transform.position;

        // Spawn area
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.15f);
        Gizmos.DrawSphere(center, spawnRadius);
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.8f);
        Gizmos.DrawWireSphere(center, spawnRadius);

        // Min player distance exclusion zone
        if (Application.isPlaying && player != null)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
            Gizmos.DrawWireSphere(player.position, minSpawnDistanceFromPlayer);
        }
    }
}
