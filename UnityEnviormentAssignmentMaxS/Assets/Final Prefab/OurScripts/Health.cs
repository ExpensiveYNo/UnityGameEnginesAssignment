using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("I-Frames")]
    public float iFrameDuration = 1.5f;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHealthChanged;
    public UnityEvent onHit;

    private int currentHealth;
    private bool isInvincible = false;

    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}/{maxHealth}");

        onHit?.Invoke();
        onHealthChanged?.Invoke(); 

        if (currentHealth <= 0)
            Die();
        else if (gameObject.CompareTag("Player"))
            StartCoroutine(iFrames());
    }

    IEnumerator iFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke();

        Debug.Log($"{gameObject.name} healed {amount} HP. HP: {currentHealth}/{maxHealth}");
    }

    void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            PlayerDied();
        }
        else
        {
            Debug.Log($"{gameObject.name} died.");
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    private void PlayerDied()
    {
        LevelManager.instance.GameOver();
    }
}