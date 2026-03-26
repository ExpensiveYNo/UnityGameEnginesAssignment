using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] collectiblePrefabs;  
    public int maxCollectiblesAtOnce = 5;    // Won't spawn more than this at a time
    public float minSpawnInterval = 5f;      // Min seconds between spawns
    public float maxSpawnInterval = 15f;     // Max seconds between spawns

    [Header("Spawn Area")]
    public float spawnRadius = 30f;          // Radius around this object to try spawning in
    public float navMeshSampleDistance = 5f; // How far to search for a valid NavMesh point
    public float spawnHeightOffset = 0.5f;   // Lifts the collectible above the floor

    [Header("Optional")]
    public Transform spawnCenter;            // Leave empty to use this object's position

    private List<GameObject> activeCollectibles = new List<GameObject>();

    void Start()
    {
        if (spawnCenter == null)
            spawnCenter = transform;

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Wait a random interval before next spawn attempt
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // Clean up any collectibles that were picked up (destroyed)
            activeCollectibles.RemoveAll(c => c == null);

            // Only spawn if we're under the cap
            if (activeCollectibles.Count < maxCollectiblesAtOnce)
                TrySpawn();
        }
    }

    void TrySpawn()
    {
        // Pick a random point within the spawn radius
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0f; // Keep it horizontal
        Vector3 candidatePoint = spawnCenter.position + randomOffset;

        // Snap it to the nearest NavMesh position so it lands on walkable ground
        if (NavMesh.SamplePosition(candidatePoint, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            Vector3 spawnPos = hit.position + Vector3.up * spawnHeightOffset;
            SpawnCollectible(spawnPos);
        }
        else
        {
            Debug.Log("CollectibleSpawner: Couldn't find a valid NavMesh point, skipping this spawn.");
        }
    }

    void SpawnCollectible(Vector3 position)
    {
        if (collectiblePrefabs.Length == 0)
        {
            Debug.LogWarning("CollectibleSpawner: No prefabs assigned!");
            return;
        }

        // Pick a random prefab from the list
        GameObject prefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
        GameObject spawned = Instantiate(prefab, position, Quaternion.identity);
        activeCollectibles.Add(spawned);

        Debug.Log($"Spawned {spawned.name} at {position}");
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.2f);
        Vector3 center = spawnCenter != null ? spawnCenter.position : transform.position;
        Gizmos.DrawSphere(center, spawnRadius);
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.8f);
        Gizmos.DrawWireSphere(center, spawnRadius);
    }
}