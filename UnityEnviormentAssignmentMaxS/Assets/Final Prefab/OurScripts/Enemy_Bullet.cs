using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;        // Damage to player
    public float safeDelay = 0.1f; // Time to ignore collisions at spawn

    private Collider bulletCollider;

    private void Awake()
    {
        //bulletCollider = GetComponent<Collider>();

        //// Temporarily disable the collider to avoid spawning inside the enemy
        //if (bulletCollider != null)
        //{
        //    bulletCollider.enabled = false;
        //    Invoke(nameof(EnableCollider), safeDelay);
        //}
    }

    private void EnableCollider()
    {
        if (bulletCollider != null)
            bulletCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only damage the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                Destroy(gameObject);
                playerHealth.TakeDamage(damage);
            }
        }

        // Destroy the bullet after hitting anything
        Destroy(gameObject);
    }
}