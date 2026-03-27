using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Text healthText;

    private int simulatedHealth;

    void Start()
    {
        if (playerHealth != null)
        {
            simulatedHealth = playerHealth.maxHealth;
            UpdateHealthText();
        }
    }

    // Call this instead of playerHealth.TakeDamage()
    public void TakeDamage(int amount)
    {
        if (playerHealth != null)
        {
            simulatedHealth -= amount;
            simulatedHealth = Mathf.Max(simulatedHealth, 0);
            UpdateHealthText();

            // Actually deal damage
            playerHealth.TakeDamage(amount);
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{simulatedHealth} / {playerHealth.maxHealth}";
        }
    }
}