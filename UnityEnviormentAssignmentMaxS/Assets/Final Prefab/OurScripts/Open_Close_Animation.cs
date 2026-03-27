using UnityEngine;
using System.Collections;

public class Open_Close_Animation : MonoBehaviour
{
    [SerializeField] GameObject door;
    private Animator mAnimator;
    public AudioClip moveSound;

    [Header("Lock Settings")]
    public bool requiresKey = true;

    [Header("Collectable Settings")]
    public GameObject collectablePrefab;
    public Transform collectableSpawnPoint;
    public float respawnTime = 60f;         // Seconds before a new collectable can spawn after the previous was collected

    private bool flag = false;
    private bool is_open = false;
    private bool collectableAvailable = true;
    private bool is_unlocked = false;
    private GameObject currentCollectable;

    void Start()
    {
        if (door != null)
            mAnimator = door.GetComponent<Animator>();
        else
            Debug.LogWarning($"{gameObject.name}: No door GameObject assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            flag = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            flag = false;
    }

    void Update()
    {
        if (mAnimator == null || !flag || !Input.GetKeyDown(KeyCode.E))
            return;

        if (requiresKey && !is_unlocked)
        {
            if (KeyHubScript.instance.UseKey())
            {
                is_unlocked = true;
                Debug.Log($"{gameObject.name} unlocked!");
                OpenDoor();
            }
            else
            {
                Debug.Log("You don't have a key!");
            }
            return;
        }

        if (!is_open)
            OpenDoor();
        else
            CloseDoor();
    }

    void OpenDoor()
    {
        mAnimator.SetTrigger("TriOpen");
        if (moveSound) AudioSource.PlayClipAtPoint(moveSound, transform.position);
        is_open = true;

        TrySpawnCollectable();
    }

    void CloseDoor()
    {
        mAnimator.SetTrigger("TriClose");
        if (moveSound) AudioSource.PlayClipAtPoint(moveSound, transform.position);
        is_open = false;
    }

    void TrySpawnCollectable()
    {
        // Don't spawn if no prefab set, already one in the locker, or timer hasn't expired
        if (collectablePrefab == null || !collectableAvailable) return;

        // Use the spawn point if assigned, otherwise fall back to this object's position
        Vector3 spawnPos = collectableSpawnPoint != null ? collectableSpawnPoint.position : transform.position;

        currentCollectable = Instantiate(collectablePrefab, spawnPos, Quaternion.identity);
        collectableAvailable = false;

        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        // Wait until the spawned collectable has been picked up (destroyed), then start the timer
        yield return new WaitUntil(() => currentCollectable == null);

        Debug.Log($"{gameObject.name}: Collectable picked up, respawning in {respawnTime}s");
        yield return new WaitForSeconds(respawnTime);

        collectableAvailable = true;
        is_unlocked = false;
        Debug.Log($"{gameObject.name}: Collectable respawned, locker relocked.");

        // Close and relock the locker if it's still open
        if (is_open)
            CloseDoor();
    }
}