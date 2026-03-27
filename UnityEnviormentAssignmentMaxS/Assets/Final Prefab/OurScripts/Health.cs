using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHealthChanged;

    private int currentHealth;

    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        onHealthChanged?.Invoke();

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return; // Don't heal the dead

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Clamp to maxHealth
        onHealthChanged?.Invoke();

        Debug.Log($"{gameObject.name} healed {amount} HP. HP: {currentHealth}/{maxHealth}");
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}