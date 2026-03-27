using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10; // Amount of damage the projectile does

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit the player
        Health playerHealth = collision.collider.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}
