using UnityEngine;
using UnityEngine.UI;

public class KeyHubScript : MonoBehaviour
{
    public static KeyHubScript instance;
    public Text keyScore;

    int key = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddPoint()
    {
        key += 1;
        UpdateUI();
    }

    // Called by clean lockers, returns true and spends a key if the player has one
    public bool UseKey()
    {
        if (key <= 0)
            return false;

        key -= 1;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        keyScore.text = key + (key == 1 ? " Key" : " Keys");
    }
}
