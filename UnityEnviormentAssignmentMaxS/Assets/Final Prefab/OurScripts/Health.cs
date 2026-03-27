using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("Events")]
    public UnityEvent onDeath;           // Hook up death events in inspector
    public UnityEvent onHealthChanged;   // Invoked whenever health changes

    private int currentHealth;

    // Public read-only property to access current health
    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(); // Initialize UI or other listeners
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // Clamp to 0

        // Notify listeners that health changed
        onHealthChanged?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}