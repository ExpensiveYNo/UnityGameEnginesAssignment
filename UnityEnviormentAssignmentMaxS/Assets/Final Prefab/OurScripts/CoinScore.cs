using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    public static CoinScore instance;
    public Text coinScore;

    int coin = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinScore.text = "Score: " + coin.ToString();
    }

    // Update is called once per frame
    public void AddPoint()
    {
        coin += 1;
        coinScore.text = "Score: " + coin.ToString();
    }
}