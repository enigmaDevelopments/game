/*----------------------------------------------------------------------------------------------------UpgradeManager------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenuPanel;
    public Text upgradeLabel;
    public TMP_Text playerCurrencyText;

    public CurrencyManager currencyManager;

    public PlayerInput input;

    // Upgrade Buttons and costs
    public Button criticalInstinctsButton; public int criticalInstinctsCost = 3; private CriticalInstinctsUpgrade criticalInstinctsUpgrade;
    public Button chainReactionButton; public int chainReactionCost = 3; private ChainReactionUpgrade chainReactionUpgrade;
    public Button barrierSurgeButton; public int barrierSurgeCost = 3; private BarrierSurgeUpgrade barrierSurgeUpgrade; public BarrierScript playerBarrier;
    public Button energyRecyclerButton; public int energyRecyclerCost = 200; private EnergyRecyclerUpgrade energyRecyclerUpgrade;
    public Button evasiveMomentumButton; public int evasiveMomentumCost = 250; private EvasiveMomentumUpgrade evasiveMomentumUpgrade;
    public Button guardianGraceButton; public int guardianGraceCost = 300; private GuardianGraceUpgrade guardianGraceUpgrade;
    public Button momentumButton; public int momentumCost = 200; private MomentumUpgrade momentumUpgrade;
    public Button reactiveArmorButton; public int reactiveArmorCost = 250; private ReactiveArmorUpgrade reactiveArmorUpgrade;
    public Button secondWindButton; public int secondWindCost = 400; private SecondWindUpgrade secondWindUpgrade;
    public Button speedForceButton; public int speedForceCost = 300; private SpeedForceUpgrade speedForceUpgrade;
    public Button temporalEchoButton; public int temporalEchoCost = 500; private TemporalEchoUpgrade temporalEchoUpgrade;

    private static UpgradeManager instance;

    private InputAction upgradeMenu;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        var InputAction = input.actions;
        InputAction.FindAction("InGameUpgradeMenu");
    }

    private void Update()
    {
        if (upgradeMenu.WasPerformedThisFrame())
        {
            ShowUpgradeMenu();
        }
    }

    public void RegisterPlayer(GameObject newPlayer)
    {
        playerBarrier = newPlayer.GetComponent<BarrierScript>();
        criticalInstinctsUpgrade = newPlayer.GetComponent<CriticalInstinctsUpgrade>();
        chainReactionUpgrade = newPlayer.GetComponent<ChainReactionUpgrade>();
        barrierSurgeUpgrade = newPlayer.GetComponent<BarrierSurgeUpgrade>();
        energyRecyclerUpgrade = newPlayer.GetComponent<EnergyRecyclerUpgrade>();
        evasiveMomentumUpgrade = newPlayer.GetComponent<EvasiveMomentumUpgrade>();
        guardianGraceUpgrade = newPlayer.GetComponent<GuardianGraceUpgrade>();
        momentumUpgrade = newPlayer.GetComponent<MomentumUpgrade>();
        reactiveArmorUpgrade = newPlayer.GetComponent<ReactiveArmorUpgrade>();
        secondWindUpgrade = newPlayer.GetComponent<SecondWindUpgrade>();
        speedForceUpgrade = newPlayer.GetComponent<SpeedForceUpgrade>();
        temporalEchoUpgrade = newPlayer.GetComponent<TemporalEchoUpgrade>();

        Debug.Log("Player registered with UpgradeManager");
    }

    private void Start()
    {
        UpdateCurrencyDisplay();
        UpdateUpgradeUI();

        // Add button listeners
        criticalInstinctsButton.onClick.AddListener(OnCriticalInstinctsUpgrade);
        chainReactionButton.onClick.AddListener(OnChainReactionUpgrade);
        barrierSurgeButton.onClick.AddListener(OnBarrierSurgeUpgrade);
        energyRecyclerButton.onClick.AddListener(OnEnergyRecyclerUpgrade);
        evasiveMomentumButton.onClick.AddListener(OnEvasiveMomentumUpgrade);
        guardianGraceButton.onClick.AddListener(OnGuardianGraceUpgrade);
        momentumButton.onClick.AddListener(OnMomentumUpgrade);
        reactiveArmorButton.onClick.AddListener(OnReactiveArmorUpgrade);
        secondWindButton.onClick.AddListener(OnSecondWindUpgrade);
        speedForceButton.onClick.AddListener(OnSpeedForceUpgrade);
        temporalEchoButton.onClick.AddListener(OnTemporalEchoUpgrade);
    }

    public void UpdateCurrencyDisplay()
    {
        if (currencyManager != null)
            playerCurrencyText.text = $"Currency: {currencyManager.GetCoinCount()}";
    }

    private void UpdateUpgradeUI()
    {
        if (currencyManager == null) return;

        // Enable/disable buttons based on whether the player can afford them
        criticalInstinctsButton.interactable = currencyManager.GetCoinCount() >= criticalInstinctsCost;
        chainReactionButton.interactable = currencyManager.GetCoinCount() >= chainReactionCost;
        barrierSurgeButton.interactable = currencyManager.GetCoinCount() >= barrierSurgeCost;
        energyRecyclerButton.interactable = currencyManager.GetCoinCount() >= energyRecyclerCost;
        evasiveMomentumButton.interactable = currencyManager.GetCoinCount() >= evasiveMomentumCost;
        guardianGraceButton.interactable = currencyManager.GetCoinCount() >= guardianGraceCost;
        momentumButton.interactable = currencyManager.GetCoinCount() >= momentumCost;
        reactiveArmorButton.interactable = currencyManager.GetCoinCount() >= reactiveArmorCost;
        secondWindButton.interactable = currencyManager.GetCoinCount() >= secondWindCost;
        speedForceButton.interactable = currencyManager.GetCoinCount() >= speedForceCost;
        temporalEchoButton.interactable = currencyManager.GetCoinCount() >= temporalEchoCost;
    }

    public void ShowUpgradeMenu()
    {
        upgradeMenuPanel.SetActive(true);
        UpdateCurrencyDisplay();
        UpdateUpgradeUI();
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenuPanel.SetActive(false);
    }

    /*----------------------------------------------------------------------------------------------------Upgrade Handlers------------------------------------------------------------------------------------------*/
    private void HandleUpgrade(Button button, int cost, System.Action activateMethod)
    {
        if (currencyManager != null && currencyManager.SpendCoins(cost))
        {
            activateMethod?.Invoke();
            button.interactable = false;
            UpdateCurrencyDisplay();
            UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough currency for Upgrade!");
        }
    }

    private void OnCriticalInstinctsUpgrade() => HandleUpgrade(criticalInstinctsButton, criticalInstinctsCost, ActivateCriticalInstinctsUpgrade);
    private void OnChainReactionUpgrade() => HandleUpgrade(chainReactionButton, chainReactionCost, ActivateChainReactionUpgrade);
    private void OnBarrierSurgeUpgrade() => HandleUpgrade(barrierSurgeButton, barrierSurgeCost, ActivateBarrierSurgeUpgrade);
    private void OnEnergyRecyclerUpgrade() => HandleUpgrade(energyRecyclerButton, energyRecyclerCost, ActivateEnergyRecyclerUpgrade);
    private void OnEvasiveMomentumUpgrade() => HandleUpgrade(evasiveMomentumButton, evasiveMomentumCost, ActivateEvasiveMomentumUpgrade);
    private void OnGuardianGraceUpgrade() => HandleUpgrade(guardianGraceButton, guardianGraceCost, ActivateGuardianGraceUpgrade);
    private void OnMomentumUpgrade() => HandleUpgrade(momentumButton, momentumCost, ActivateMomentumUpgrade);
    private void OnReactiveArmorUpgrade() => HandleUpgrade(reactiveArmorButton, reactiveArmorCost, ActivateReactiveArmorUpgrade);
    private void OnSecondWindUpgrade() => HandleUpgrade(secondWindButton, secondWindCost, ActivateSecondWindUpgrade);
    private void OnSpeedForceUpgrade() => HandleUpgrade(speedForceButton, speedForceCost, ActivateSpeedForceUpgrade);
    private void OnTemporalEchoUpgrade() => HandleUpgrade(temporalEchoButton, temporalEchoCost, ActivateTemporalEchoUpgrade);

    /*----------------------------------------------------------------------------------------------------Activate Methods------------------------------------------------------------------------------------------*/
    private void ActivateCriticalInstinctsUpgrade() { criticalInstinctsUpgrade?.Activate(); Debug.Log("Critical Instincts Activated!"); }
    private void ActivateChainReactionUpgrade() { chainReactionUpgrade?.Activate(); Debug.Log("Chain Reaction Activated!"); }
    private void ActivateBarrierSurgeUpgrade() { if (playerBarrier != null) barrierSurgeUpgrade?.Activate(playerBarrier); }
    private void ActivateEnergyRecyclerUpgrade() { energyRecyclerUpgrade?.Activate(); Debug.Log("Energy Recycler Activated!"); }
    private void ActivateEvasiveMomentumUpgrade() { evasiveMomentumUpgrade?.Activate(); Debug.Log("Evasive Momentum Activated!"); }
    private void ActivateGuardianGraceUpgrade() { guardianGraceUpgrade?.Activate(); Debug.Log("Guardian Grace Activated!"); }
    private void ActivateMomentumUpgrade() { momentumUpgrade?.Activate(); Debug.Log("Momentum Activated!"); }
    private void ActivateReactiveArmorUpgrade() { reactiveArmorUpgrade?.Activate(); Debug.Log("Reactive Armor Activated!"); }
    private void ActivateSecondWindUpgrade() { secondWindUpgrade?.Activate(); Debug.Log("Second Wind Activated!"); }
    private void ActivateSpeedForceUpgrade() { speedForceUpgrade?.Activate(); Debug.Log("Speed Force Activated!"); }
    private void ActivateTemporalEchoUpgrade() { temporalEchoUpgrade?.Activate(); Debug.Log("Temporal Echo Activated!"); }
}
