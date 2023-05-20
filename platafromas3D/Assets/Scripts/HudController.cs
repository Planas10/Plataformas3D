using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HudController : MonoBehaviour
{
    public static HudController instance;
    public TextMeshProUGUI coinText;
    public PlayerController player;
    public List<Coin> coins = new List<Coin>();
    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<PlayerController>();
        coins.AddRange(FindObjectsOfType<Coin>());
        UpdateCoins(0);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void UpdateCoins(int coinValue) 
    {
        coinText.text = coinValue.ToString() + " / " + coins.Count;
        if (coinValue >= coins.Count) 
        {
            SceneManager.LoadScene("WinningScene");
        }
    }

}
