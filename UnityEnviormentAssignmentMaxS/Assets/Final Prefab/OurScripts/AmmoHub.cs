using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("References")]
    public RaycastShooter playerShooter; // Reference to your RaycastShooter script
    public TextMeshProUGUI ammoText;      // Reference to the TextMeshProUGUI component

    void Start()
    {
        // Initial update
        UpdateAmmoText();
    }

    void Update()
    {
        // Update every frame (simple way)
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (playerShooter != null && ammoText != null)
        {
            ammoText.text = playerShooter.currentAmmo + " / " + playerShooter.maxAmmo;
        }
    }
}