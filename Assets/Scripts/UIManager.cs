using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("General")]
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private Image levelProgressionSlider;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshPro scoreBoardText;
    [SerializeField] private TextMeshPro oldScoreBoardText;
    [SerializeField] private TextMeshProUGUI moneyPickupText;
    [SerializeField] private Animator moneyPickupAnim;
    [SerializeField] private GameObject generalCanvas;
    [SerializeField] private GameObject endGamePanel;

    [Header("Upgrade")]
    [SerializeField] private TextMeshProUGUI staminaLevelText;

    [SerializeField] private TextMeshProUGUI staminaCostText;
    [SerializeField] private Button staminaButton;

    [Space]
    [SerializeField] private TextMeshProUGUI incomeLevelText;

    [SerializeField] private TextMeshProUGUI incomeCostText;
    [SerializeField] private Button incomeButton;

    [Space]
    [SerializeField] private TextMeshProUGUI speedLevelText;

    [SerializeField] private TextMeshProUGUI speedCostText;
    [SerializeField] private Button speedButton;

    [Header("Pause Panel")]
    [SerializeField] private GameObject musicToggle;

    [SerializeField] private GameObject vibrateToggle;

    public GameObject EndGamePanel { get => endGamePanel; }
    public GameObject GeneralCanvas { get => generalCanvas; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        moneyPickupText.text = "";
    }

    public void SetupUI(int gameLevel, float currentDistance, float maxDistance, float money, int staminaLevel, float staminaCost, int incomeLevel, float incomeCost, int speedLevel, float speedCost)
    {
        UpdateLevelText(gameLevel);
        UpdateLevelProgressionSlider(currentDistance, maxDistance);
        UpdateMoneyText(money);
        UpdateAllUpgradePanels(staminaLevel, staminaCost, incomeLevel, incomeCost, speedLevel, speedCost);
        UpdateScoreBoard(currentDistance, maxDistance);
    }

    #region Update UI

    public void PlayMoneyPickupUI(float money)
    {
        moneyPickupText.text = "$ " + money.ToString("F1");
        moneyPickupAnim.SetTrigger("action");
    }

    public void SetupPausePanel(bool canPlayMusic, bool canVibrate)
    {
        if (canPlayMusic)
            musicToggle.SetActive(true);
        else
            musicToggle.SetActive(false);

        if (canVibrate)
            vibrateToggle.SetActive(true);
        else
            vibrateToggle.SetActive(false);
    }

    public void UpdateScoreBoard(float currentDistance, float maxDistance)
    {
        float distance = maxDistance - currentDistance;
        scoreBoardText.text = "-" + distance.ToString("F1") + " m";
    }

    public void UpdateOldScoreBoard(float currentDistance, float maxDistance)
    {
        float distance = maxDistance - currentDistance;
        oldScoreBoardText.text = "-" + distance.ToString("F1") + " m";
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }

    public void UpdateLevelProgressionSlider(float currentDistance, float maxDistance)
    {
        float normalizedDistance = currentDistance / maxDistance;
        levelProgressionSlider.fillAmount = normalizedDistance;
    }

    public void UpdateMoneyText(float money)
    {
        moneyText.text = (money).ToString("F1");
    }

    public void UpdateUpgradePanel(TextMeshProUGUI levelText, TextMeshProUGUI costText, Button costButton, int level, float cost)
    {
        levelText.text = "LVL " + level;
        costText.text = ((int)cost).ToString();
        if (GameManager.instance.HasMoney(cost))
            costButton.interactable = true;
        else
            costButton.interactable = false;
    }

    public void UpdateAllUpgradePanels(int staminaLevel, float staminaCost, int incomeLevel, float incomeCost, int speedLevel, float speedCost)
    {
        UpdateUpgradePanel(staminaLevelText, staminaCostText, staminaButton, staminaLevel, staminaCost);
        UpdateUpgradePanel(incomeLevelText, incomeCostText, incomeButton, incomeLevel, incomeCost);
        UpdateUpgradePanel(speedLevelText, speedCostText, speedButton, speedLevel, speedCost);
    }

    #endregion Update UI

    #region Buttons

    public void UpgradeStaminaButton()
    {
        GameManager.instance.Buy(GameManager.instance.GetCost(BuyType.staminaUpgrade), BuyType.staminaUpgrade);

        UpdateAllUpgradePanels(staminaLevel: GameManager.instance.Data.staminaLevel + 1,
            staminaCost: GameManager.instance.GetCost(BuyType.staminaUpgrade),
            incomeLevel: GameManager.instance.Data.incomeLevel + 1,
            incomeCost: GameManager.instance.GetCost(BuyType.incomeUpgrade),
            speedLevel: GameManager.instance.Data.speedLevel + 1,
            speedCost: GameManager.instance.GetCost(BuyType.speedUpgrade));
        UpdateMoneyText(GameManager.instance.Data.money);
    }

    public void UpgradeIncomeButton()
    {
        GameManager.instance.Buy(GameManager.instance.GetCost(BuyType.incomeUpgrade), BuyType.incomeUpgrade);

        UpdateAllUpgradePanels(staminaLevel: GameManager.instance.Data.staminaLevel + 1,
            staminaCost: GameManager.instance.GetCost(BuyType.staminaUpgrade),
            incomeLevel: GameManager.instance.Data.incomeLevel + 1,
            incomeCost: GameManager.instance.GetCost(BuyType.incomeUpgrade),
            speedLevel: GameManager.instance.Data.speedLevel + 1,
            speedCost: GameManager.instance.GetCost(BuyType.speedUpgrade));
        UpdateMoneyText(GameManager.instance.Data.money);
    }

    public void UpgradeSpeedButton()
    {
        GameManager.instance.Buy(GameManager.instance.GetCost(BuyType.speedUpgrade), BuyType.speedUpgrade);

        UpdateAllUpgradePanels(staminaLevel: GameManager.instance.Data.staminaLevel + 1,
            staminaCost: GameManager.instance.GetCost(BuyType.staminaUpgrade),
            incomeLevel: GameManager.instance.Data.incomeLevel + 1,
            incomeCost: GameManager.instance.GetCost(BuyType.incomeUpgrade),
            speedLevel: GameManager.instance.Data.speedLevel + 1,
            speedCost: GameManager.instance.GetCost(BuyType.speedUpgrade));
        UpdateMoneyText(GameManager.instance.Data.money);
    }

    public void CloseStartPanelButton()
    {
        GameManager.instance.canPlay = true;
    }

    public void SetVolumeButton()
    {
        GameManager.instance.Data.hasAudioVolume = !GameManager.instance.Data.hasAudioVolume;
        AudioManager.instance.SetMusic(GameManager.instance.Data.hasAudioVolume);
        SetupPausePanel(GameManager.instance.Data.hasAudioVolume, GameManager.instance.Data.canVibrate);
    }

    public void SetVibrateButton()
    {
        GameManager.instance.Data.canVibrate = !GameManager.instance.Data.canVibrate;
        SetupPausePanel(GameManager.instance.Data.hasAudioVolume, GameManager.instance.Data.canVibrate);
    }

    public void PauseButton()
    {
        GameManager.instance.canPlay = false;
        SetupPausePanel(GameManager.instance.Data.hasAudioVolume, GameManager.instance.Data.canVibrate);
    }

    public void ResumeButton()
    {
        GameManager.instance.canPlay = true;
        Time.timeScale = 1;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DataPersistenceManager.instance.SaveGame();
    }

    #endregion Buttons
}