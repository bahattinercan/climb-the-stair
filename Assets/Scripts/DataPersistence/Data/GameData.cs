using UnityEngine;

[System.Serializable]
public class GameData
{
    public float money;
    public int level;
    public int staminaLevel;
    public int incomeLevel;
    public int speedLevel;
    public float oldDistance;
    public bool hasAudioVolume;
    public bool canVibrate;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        money = 35;
        level = 1;
        staminaLevel = 0;
        incomeLevel = 0;
        speedLevel = 0;
        oldDistance = 0f;
        hasAudioVolume = true;
        canVibrate = true;
    }
}