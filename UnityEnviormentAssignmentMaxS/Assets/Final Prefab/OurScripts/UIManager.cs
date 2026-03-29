using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;

    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    // Wire these up to your Respawn and Main Menu buttons in the Inspector
    public void OnRespawnPressed()
    {
        LevelManager.instance.Respawn();
    }

    public void OnMainMenuPressed()
    {
        LevelManager.instance.GoToMainMenu();
    }
}