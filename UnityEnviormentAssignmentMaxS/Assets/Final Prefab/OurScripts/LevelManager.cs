using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        // Unlock and show the cursor so the user can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
            _ui.ToggleDeathPanel();
        // Freeze the game
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        // Reload the current scene and unfreeze
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        // Unfreeze before leaving
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
