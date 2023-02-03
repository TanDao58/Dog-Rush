using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int currentLevel = 0;
    public bool isBuyRemoveAds = false;
}


public class PlayerDataManager : Singleton<PlayerDataManager>
{
    PlayerData playerData = new PlayerData();
    private void OnEnable()
    {
        LoadPlayerData();
    }
    public bool GetRemoveAdsStatus()
    {
        return playerData.isBuyRemoveAds;
    }
    public void SetRemoveAdsStatus(bool status)
    {
        playerData.isBuyRemoveAds = status;
        SavePlayerData();
    }
    public int GetCurrentLevel()
    {
        return playerData.currentLevel;
    }
    public void SetCurrentLevel(int newLevel)
    {
        playerData.currentLevel = newLevel;
        SavePlayerData();
    }
    public void LoadPlayerData()
    {
        playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
    }
    public void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "player-data.txt");
    }
}
