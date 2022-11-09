using DG.Tweening;
using UnityEngine;

public enum BuyType
{
    staminaUpgrade,
    incomeUpgrade,
    speedUpgrade,
}

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;
    public Background background;

    [Header("Player")]
    public Transform player;

    public Transform[] playerParts;
    public GameObject playerSweatVFX;
    private bool isPlayerSweating;

    [Space]
    [SerializeField] private ColorChanger colorChanger;

    [SerializeField] private float[] staminaUpgradeCosts;
    [SerializeField] private float[] incomeUpgradeCosts;
    [SerializeField] private float[] speedUpgradeCosts;
    [SerializeField] private float[] levelDistances;

    private GameData data;
    private float staminaMultiplier = 10f;
    private float incomeMultiplier = .1f;
    private float speedMultiplier = .15f;

    private float stamina;
    private float income;
    private float speed;

    private float baseStamina = 100f;
    private float baseIncome = 0.5f;
    private float baseSpeed = 1f;

    private float maxStamina;
    private float staminaDecrease = 5f;

    private float currentDistance = 0;
    private float maxDistance;
    private const float stepHeight = .2f;

    public bool canPlay = false;

    public GameData Data { get => data; }
    public float Distance { get => currentDistance; set => currentDistance = value; }
    public float MaxDistance { get => maxDistance; set => maxDistance = value; }
    public float Stamina { get => stamina; set => stamina = value; }

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        Stamina = baseStamina;
        income = baseIncome;
        speed = baseSpeed;
    }

    private void SetupGame()
    {
        isPlayerSweating = false;
        maxDistance = levelDistances[data.level - 1];

        Stamina = baseStamina + staminaMultiplier * data.staminaLevel;
        maxStamina = stamina;
        income = baseIncome + incomeMultiplier * data.incomeLevel;
        speed = baseSpeed + speedMultiplier * data.speedLevel;
        Time.timeScale = speed;

        CreateOldScoreNewel();

        // UI
        float staminaCost = staminaUpgradeCosts[Data.staminaLevel];
        float incomeCost = incomeUpgradeCosts[Data.incomeLevel];
        float speedCost = speedUpgradeCosts[Data.speedLevel];
        UIManager.instance.SetupUI(
            gameLevel: data.level,
            currentDistance: 0,
            maxDistance: 500,
            money: data.money,
            staminaLevel: data.staminaLevel + 1,
            staminaCost: staminaCost,
            incomeLevel: data.incomeLevel + 1,
            incomeCost: incomeCost,
            speedLevel: data.speedLevel + 1,
            speedCost: speedCost);

        // volume and vibrate
        AudioManager.instance.SetMusic(data.hasAudioVolume);

        // playerColorChanger
        colorChanger.SetupColorChanger(maxStamina);
    }

    public void StepUp()
    {
        Stamina -= staminaDecrease;
        if (stamina < 0)
        {
            GameOver();
            return;
        }
        data.money += income;
        currentDistance += stepHeight;
        if (stamina <= maxStamina * .5f && isPlayerSweating == false)
        {
            isPlayerSweating = true;
            foreach (Transform bodyPart in playerParts)
            {
                bodyPart.DOScale(Vector3.one * 1.25f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            playerSweatVFX.SetActive(true);
        }

        UIManager.instance.UpdateScoreBoard(currentDistance, MaxDistance);
        UIManager.instance.UpdateLevelProgressionSlider(currentDistance, MaxDistance);
        UIManager.instance.UpdateMoneyText(data.money);
        UIManager.instance.PlayMoneyPickupUI(income);
        if (!colorChanger.HasCalculated)
            colorChanger.SetupColorChanger(maxStamina);
        colorChanger.ChangeToEndColor(staminaDecrease);
        CancelInvoke("RegainStamina");
        InvokeRepeating("RegainStamina", 2f, .3f);
    }

    private void RegainStamina()
    {
        Stamina += staminaDecrease;
        if (Stamina > maxStamina*.75f)
        {
            foreach (Transform bodyPart in playerParts)
            {
                DOTween.Kill(bodyPart);
                bodyPart.DOScale(Vector3.one, 1.25f);
            }
            isPlayerSweating = false;
            playerSweatVFX.SetActive(false);
        }

        if (Stamina < maxStamina)
        {
            colorChanger.ChangeToStartColor(staminaDecrease);
        }
        if (Stamina >= maxStamina)
        {
            Stamina = maxStamina;
            CancelInvoke("RegainStamina");
        }
    }

    private void CreateOldScoreNewel()
    {
        int stepNumber = (int)(data.oldDistance / stepHeight);
        UIManager.instance.UpdateOldScoreBoard(data.oldDistance, MaxDistance);
        for (int i = 0; i < stepNumber; i++)
        {
            StairSpawner.instance.SpawnOldStairNewel();
        }
    }

    public bool HasMoney(float cost)
    {
        if (data.money >= cost)
            return true;
        return false;
    }

    public void Buy(float cost, BuyType buyType)
    {
        switch (buyType)
        {
            case BuyType.staminaUpgrade:
                data.staminaLevel++;
                Stamina = baseStamina + staminaMultiplier * data.staminaLevel;
                maxStamina = stamina;
                colorChanger.SetupColorChanger(maxStamina);
                break;

            case BuyType.incomeUpgrade:
                data.incomeLevel++;
                income = baseIncome + incomeMultiplier * data.incomeLevel;
                break;

            case BuyType.speedUpgrade:
                data.speedLevel++;
                speed = baseSpeed + speedMultiplier * data.speedLevel;
                Time.timeScale = speed;
                break;
        }
        data.money -= cost;
    }

    public float GetCost(BuyType buyType)
    {
        switch (buyType)
        {
            case BuyType.staminaUpgrade:
                return staminaUpgradeCosts[Data.staminaLevel];

            case BuyType.incomeUpgrade:
                return incomeUpgradeCosts[Data.incomeLevel];

            case BuyType.speedUpgrade:
                return speedUpgradeCosts[Data.speedLevel];

            default:
                return 0;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        player.gameObject.SetActive(false);
        UIManager.instance.GeneralCanvas.SetActive(false);
        UIManager.instance.EndGamePanel.SetActive(true);
        canPlay = false;

    }

    public void LoadData(GameData data)
    {
        this.data = data;
        SetupGame();
    }

    public void SaveData(GameData data)
    {
        if (data.oldDistance < Distance)
        {
            data.oldDistance = Distance;
        }
    }
}