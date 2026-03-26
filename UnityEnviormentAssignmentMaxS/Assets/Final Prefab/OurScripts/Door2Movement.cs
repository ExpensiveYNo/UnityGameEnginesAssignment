using System.Collections;
using UnityEngine;

public class Door2Movement : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] AudioClip doorSqueak;

    [Header("Ammo Pack Settings")]
    public GameObject ammoPackPrefab;       
    public Transform ammoSpawnPoint;        
    public float respawnTime = 60f;         //Time in seconds before a new ammo pack can spawn after the previous one was collected

    Animator anim;
    AudioSource source;

    private bool hasBeenOpened = false;     // Has the locker ever been opened?
    private bool ammoAvailable = true;      // Is there currently an ammo pack to spawn?
    private GameObject spawnedAmmoPack;     // Reference to the live ammo pack

    private void Start()
    {
        anim = door.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (anim == null)
        {
            Debug.LogWarning("Door2Movement: Animator has not been assigned.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Open");
            source.PlayOneShot(doorSqueak);

            // Spawn ammo if one is available and there isn't already one sitting inside
            if (ammoAvailable && spawnedAmmoPack == null && ammoPackPrefab != null)
            {
                Vector3 spawnPos = ammoSpawnPoint != null ? ammoSpawnPoint.position : transform.position;
                spawnedAmmoPack = Instantiate(ammoPackPrefab, spawnPos, Quaternion.identity);
                ammoAvailable = false;
                hasBeenOpened = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (anim == null)
        {
            Debug.LogWarning("Door2Movement: Animator has not been assigned.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Close");
            source.PlayOneShot(doorSqueak);

            // If the pack was collected (destroyed by player), start the respawn timer
            if (hasBeenOpened && !ammoAvailable && spawnedAmmoPack == null)
                StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);
        ammoAvailable = true;
        Debug.Log($"{gameObject.name}: Ammo pack ready to respawn.");
    }
}
