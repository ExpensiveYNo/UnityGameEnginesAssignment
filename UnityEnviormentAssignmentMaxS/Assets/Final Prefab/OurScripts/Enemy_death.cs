using UnityEngine;

public class EnemyDeathAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDeathAnimation()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");
    }
}