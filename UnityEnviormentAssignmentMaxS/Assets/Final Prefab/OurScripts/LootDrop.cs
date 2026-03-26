using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [System.Serializable]
    public class LootEntry
    {
        public GameObject prefab;           // The collectible prefab to spawn
        [Range(0f, 1f)] public float chance; // 0 = never, 1 = always (e.g. 0.3 = 30%)
    }

    [Header("Loot Table")]
    public LootEntry[] lootTable;

    [Header("Spawn Settings")]
    public float dropHeightOffset = 0.5f;   // Spawn slightly above ground so it doesn't clip

    // Hook this up to the Health onDeath UnityEvent in the Inspector
    public void Drop()
    {
        foreach (LootEntry entry in lootTable)
        {
            if (entry.prefab == null) continue;

            if (Random.value <= entry.chance)
            {
                Vector3 spawnPos = transform.position + Vector3.up * dropHeightOffset;
                Instantiate(entry.prefab, spawnPos, Quaternion.identity);
                Debug.Log($"{gameObject.name} dropped {entry.prefab.name}");
            }
        }
    }
}