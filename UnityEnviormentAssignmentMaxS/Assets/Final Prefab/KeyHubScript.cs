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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyScore.text = key.ToString() + " Key";
    }

    // Update is called once per frame
    public void AddPoint()
    {
        key += 1;
        keyScore.text = key.ToString() + " Key";
    }
}
