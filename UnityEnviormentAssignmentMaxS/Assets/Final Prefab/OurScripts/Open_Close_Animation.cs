using UnityEngine;

public class Open_Close_Animation : MonoBehaviour
{
    [SerializeField] GameObject door;
    private Animator mAnimator;
    public AudioClip moveSound;

    [Header("Lock Settings")]
    public bool requiresKey = true;   // Uncheck in Inspector for doors that don't need a key

    private bool flag = false;
    private bool is_open = false;
    private bool is_unlocked = false; // Per-door unlock state — independent on every instance

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
        if (mAnimator == null || !flag || !Input.GetKeyDown(KeyCode.O))
            return;

        // If the door needs a key and isn't unlocked yet, try to spend one
        if (requiresKey && !is_unlocked)
        {
            if (KeyHubScript.instance.UseKey())
            {
                is_unlocked = true;
                Debug.Log($"{gameObject.name} unlocked!");
                OpenDoor(); // Open immediately after unlocking
            }
            else
            {
                Debug.Log("You don't have a key!");
                // Optionally: play a "locked" sound here
            }
            return;
        }

        // locker is unlocked toggle open/close
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
    }

    void CloseDoor()
    {
        mAnimator.SetTrigger("TriClose");
        if (moveSound) AudioSource.PlayClipAtPoint(moveSound, transform.position);
        is_open = false;
    }
}