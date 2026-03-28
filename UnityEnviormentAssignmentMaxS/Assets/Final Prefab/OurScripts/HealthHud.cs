using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Health playerHealth;
    public TextMeshProUGUI healthText;

    void Start()
    {
        UpdateHealthText();

        // Subscribe to event
        playerHealth.onHealthChanged.AddListener(UpdateHealthText);
    }

    void UpdateHealthText()
    {
        healthText.text = playerHealth.CurrentHealth + " / " + playerHealth.maxHealth;
    }
}