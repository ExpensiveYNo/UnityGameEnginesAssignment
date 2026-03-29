using UnityEngine;
using System.Collections;

public class EnemyDeathHandler : MonoBehaviour
{
    [Header("Death Settings")]
    public float destroyDelay = 2f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Hook this up to the Health onDeath UnityEvent in the Inspector
    public void PlayDeathAnimation()
    {
        Debug.Log("PlayDeathAnimation called");
        if (animator != null)
        {
            StartCoroutine(CheckState());
            animator.ResetTrigger("Die"); // Ensure the trigger is reset before setting it again
            animator.SetTrigger("Die");
        }
        else
            Debug.Log("Animator is null!");

        IEnumerator CheckState()
        {
            yield return null; // wait one frame
            Debug.Log($"Is in Death: {animator.GetCurrentAnimatorStateInfo(0).IsName("Death")}");
            Debug.Log($"Is in Idle: {animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")}");
        }

        // Disable any AI, colliders etc. so the enemy stops acting while dying
        GetComponent<Collider>()?.gameObject.SetActive(false);

        Destroy(gameObject, destroyDelay);
    }
}