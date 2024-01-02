using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header(" Settings ")]
    private int _coins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinContainer();
    }

    public void AddCoins(int amount)
    {
        _coins += amount;

        UpdateCoinContainer();
        Debug.Log("We now have " + _coins + " coins");

        SaveData();
    }

    private void UpdateCoinContainer()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject coinContainer in coinContainers)
        {
            coinContainer.GetComponent<TextMeshProUGUI>().text = _coins.ToString();
        }
    }

    private void LoadData()
    {
        _coins = PlayerPrefs.GetInt("Coins");
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", _coins); 
    }
}
