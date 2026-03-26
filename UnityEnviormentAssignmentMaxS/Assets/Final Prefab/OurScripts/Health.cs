using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("Events")]
    public UnityEvent onDeath;  //Hook up death events in the inspector

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}/{maxHealth}"); //Debug log for testing

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        //Debug.Log($"{gameObject.name} died."); //Debug log for testing
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}
