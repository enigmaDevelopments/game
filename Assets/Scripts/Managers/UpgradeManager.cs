//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class UpgradeManager : MonoBehaviour
//{
//    public GameObject upgradeMenuPanel;  // Reference to the upgrade menu panel UI
//    public Text upgradeLabel;            // Label for the menu title
//    public Text playerCurrencyText;      // Text showing the player's current currency

//    // Upgrade UI elements
//    public Button criticalInstinctsButton;  // Button to activate Critical Instincts upgrade
//    private CriticalInstinctsUpgrade criticalInstinctsUpgrade;
//    public int criticalInstinctsCost = 3;

//    public Button chainReactionButton;     // Button to activate Chain Reaction upgrade
//    private ChainReactionUpgrade chainReactionUpgrade;
//    public int chainReactionCost = 3;

//    public Button barrierSurgeButton;
//    private BarrierSurgeUpgrade barrierSurgeUpgrade;
//    public int barrierSurgeCost = 3;

//    public Button energyRecyclerButton;
//    private EnergyRecyclerUpgrade energyRecyclerUpgrade;
//    public int energyRecyclerCost = 200;

//    public Button evasiveMomentumButton;
//    private EvasiveMomentumUpgrade evasiveMomentumUpgrade;
//    public int evasiveMomentumCost = 250;

//    public Button guardianGraceButton;
//    private GuardianGraceUpgrade guardianGraceUpgrade;
//    public int guardianGraceCost = 300;

//    public Button momentumButton;
//    private MomentumUpgrade momentumUpgrade;
//    public int momentumCost = 200;

//    public Button reactiveArmorButton;
//    private ReactiveArmorUpgrade reactiveArmorUpgrade;
//    public int reactiveArmorCost = 250;

//    public Button secondWindButton;
//    public int secondWindCost = 400;
//    private SecondWindUpgrade secondWindUpgrade;

//    public Button speedForceButton;
//    public int speedForceCost = 300;
//    private SpeedForceUpgrade speedForceUpgrade;

//    public Button temporalEchoButton;
//    public int temporalEchoCost = 500;
//    private TemporalEchoUpgrade temporalEchoUpgrade;

//    private int playerCurrency = 500;         // Player's current currency

//    private void Start()
//    {
//        // Initialize UI elements
//        UpdateCurrencyDisplay();
//        UpdateUpgradeUI();

//        // Add listeners to upgrade buttons
//        criticalInstinctsButton.onClick.AddListener(OnCriticalInstinctsUpgrade);
//        criticalInstinctsUpgrade = FindAnyObjectByType<CriticalInstinctsUpgrade>();
//        chainReactionButton.onClick.AddListener(OnChainReactionUpgrade);
//        chainReactionUpgrade = FindAnyObjectByType<ChainReactionUpgrade>();
//        barrierSurgeButton.onClick.AddListener(OnBarrierSurgeUpgrade);
//        barrierSurgeUpgrade = FindAnyObjectByType<BarrierSurgeUpgrade>();
//        energyRecyclerButton.onClick.AddListener(OnEnergyRecyclerUpgrade);
//        energyRecyclerUpgrade = FindAnyObjectByType<EnergyRecyclerUpgrade>();
//        evasiveMomentumButton.onClick.AddListener(OnEvasiveMomentumUpgrade);
//        evasiveMomentumUpgrade = FindAnyObjectByType<EvasiveMomentumUpgrade>();
//        guardianGraceButton.onClick.AddListener(OnGuardianGraceUpgrade);
//        guardianGraceUpgrade = FindAnyObjectByType<GuardianGraceUpgrade>();
//        momentumButton.onClick.AddListener(OnMomentumUpgrade);
//        momentumUpgrade = FindAnyObjectByType<MomentumUpgrade>();
//        reactiveArmorButton.onClick.AddListener(OnReactiveArmorUpgrade);
//        reactiveArmorUpgrade = FindAnyObjectByType<ReactiveArmorUpgrade>();
//        secondWindButton.onClick.AddListener(OnSecondWindUpgrade);
//        secondWindUpgrade = FindAnyObjectByType<SecondWindUpgrade>();
//        speedForceButton.onClick.AddListener(OnSpeedForceUpgrade);
//        speedForceUpgrade = FindAnyObjectByType<SpeedForceUpgrade>();
//        temporalEchoButton.onClick.AddListener(OnTemporalEchoUpgrade);
//        temporalEchoUpgrade = FindAnyObjectByType<TemporalEchoUpgrade>();
       
//    }


//    private void UpdateCurrencyDisplay()
//    {
//        // Update the player's current currency display
//        playerCurrencyText.text = $"Currency: {playerCurrency}";
//    }

//    private void UpdateUpgradeUI()
//    {
//        // Update the UI based on the player's current currency and the cost of upgrades
//        playerCurrencyText.text = $"Currency: {playerCurrency}";  // Update the currency display

//        // List of buttons and their corresponding costs
//        Button[] upgradeButtons = new Button[]
//        {
//        criticalInstinctsButton,
//        chainReactionButton,
//        barrierSurgeButton,
//        energyRecyclerButton,
//        evasiveMomentumButton,
//        guardianGraceButton,
//        momentumButton,
//        reactiveArmorButton,
//        secondWindButton,
//        speedForceButton,
//        temporalEchoButton,
        
//        };

//        int[] upgradeCosts = new int[]
//        {
//        criticalInstinctsCost,
//        chainReactionCost,
//        barrierSurgeCost,
//        energyRecyclerCost,
//        evasiveMomentumCost,
//        guardianGraceCost,
//        momentumCost,
//        reactiveArmorCost,
//        secondWindCost,
//        speedForceCost,
//        temporalEchoCost,
        
//        };

//        // Enable or disable buttons based on whether the player can afford the upgrade
//        for (int i = 0; i < upgradeButtons.Length; i++)
//        {
//            upgradeButtons[i].interactable = playerCurrency >= upgradeCosts[i];
//        }
//    }


//    public void ShowUpgradeMenu()
//    {
//        // Show the upgrade menu panel
//        upgradeMenuPanel.SetActive(true);
//        UpdateCurrencyDisplay();
//        UpdateUpgradeUI();
//    }

//    public void CloseUpgradeMenu()
//    {
//        // Close the upgrade menu panel
//        upgradeMenuPanel.SetActive(false);
//    }
//    /*----------------------------------------------------------------------------------------------------critical instincts------------------------------------------------------------------------------------------*/
//    private void OnCriticalInstinctsUpgrade()
//    {
//        if (playerCurrency >= criticalInstinctsCost)
//        {
//            // Deduct the cost and apply the upgrade
//            playerCurrency -= criticalInstinctsCost;
//            ActivateCriticalInstinctsUpgrade();
//            UpdateCurrencyDisplay();
//            UpdateUpgradeUI();
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Upgrade!");
//        }
//    }

//    private void ActivateCriticalInstinctsUpgrade()
//    {
//        if (criticalInstinctsUpgrade != null)
//        {
//            // Activate the Critical Instincts upgrade
//            criticalInstinctsUpgrade.ActivateCriticalInstincts(); // Call the method from CriticalInstinctsUpgrade

//            // Update UI after activation
//            criticalInstinctsButton.interactable = false;  // Disable the button after purchase
//            Debug.Log("Critical Instincts Upgrade Activated!");
//        }
//        else
//        {
//            Debug.LogWarning("CriticalInstinctsUpgrade not found!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Chain Reaction------------------------------------------------------------------------------------------*/
//    private void OnChainReactionUpgrade()
//    {
//        if (playerCurrency >= chainReactionCost)
//        {
//            // Deduct the cost and apply the upgrade
//            playerCurrency -= chainReactionCost;
//            ActivateChainReactionUpgrade();
//            UpdateCurrencyDisplay();
//            UpdateUpgradeUI();
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Upgrade!");
//        }
//    }

//    private void ActivateChainReactionUpgrade()
//    {
//        if (chainReactionUpgrade != null)
//        {
//            // Call the method to activate the chain reaction upgrade
//            chainReactionUpgrade.ActivateChainReactionUpgrade();
//            Debug.Log("Chain Reaction Upgrade Activated through Button!");
//        }
//        else
//        {
//            Debug.LogWarning("ChainReactionUpgrade not found!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Barrier Surge------------------------------------------------------------------------------------------*/

//    private void OnBarrierSurgeUpgrade()
//    {
//        if (playerCurrency >= barrierSurgeCost)
//        {
//            // Deduct the cost and apply the upgrade
//            playerCurrency -= barrierSurgeCost;
//            ActivateBarrierSurgeUpgrade();
//            UpdateCurrencyDisplay();
//            UpdateUpgradeUI();
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Upgrade!");
//        }
//    }

//    private void ActivateBarrierSurgeUpgrade()
//    {
//        if(barrierSurgeUpgrade != null)
//        {
//            barrierSurgeUpgrade.ActivateBarrierSurge();
//            Debug.Log("Barrier Surge Upgrade Activated");
//        }
//        else
//        {
//            Debug.LogWarning("Upgrade not found");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Energy Recycler------------------------------------------------------------------------------------------*/
//    private void OnEnergyRecyclerUpgrade()
//    {
//        if (playerCurrency >= energyRecyclerCost)
//        {
//            playerCurrency -= energyRecyclerCost;
//            energyRecyclerUpgrade.isActive = true;
//            UpdateCurrencyDisplay();
//            Debug.Log("Energy Recycler Upgrade Activated!");
//            energyRecyclerButton.interactable = false; // disable after buying
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Energy Recycler Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Evasive Momentum------------------------------------------------------------------------------------------*/

//    private void OnEvasiveMomentumUpgrade()
//    {
//        if (playerCurrency >= evasiveMomentumCost)
//        {
//            playerCurrency -= evasiveMomentumCost;
//            evasiveMomentumUpgrade.isActive = true;
//            UpdateCurrencyDisplay();

//            evasiveMomentumButton.interactable = false;
//            Debug.Log("Evasive Momentum Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Evasive Momentum Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Guardian Grace------------------------------------------------------------------------------------------*/
//    private void OnGuardianGraceUpgrade()
//    {
//        if (playerCurrency >= guardianGraceCost)
//        {
//            playerCurrency -= guardianGraceCost;
//            guardianGraceUpgrade.isActive = true;
//            UpdateCurrencyDisplay();

//            guardianGraceButton.interactable = false;
//            Debug.Log("Guardian Grace Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Guardian Grace Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Momentum------------------------------------------------------------------------------------------*/
//    private void OnMomentumUpgrade()
//    {
//        if (playerCurrency >= momentumCost)
//        {
//            playerCurrency -= momentumCost;
//            momentumUpgrade.isActive = true;
//            UpdateCurrencyDisplay();

//            momentumButton.interactable = false;
//            Debug.Log("Momentum Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Momentum Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Reactive Armor------------------------------------------------------------------------------------------*/
//    private void OnReactiveArmorUpgrade()
//    {
//        if (playerCurrency >= reactiveArmorCost)
//        {
//            playerCurrency -= reactiveArmorCost;
//            reactiveArmorUpgrade.isActive = true;
//            UpdateCurrencyDisplay();

//            reactiveArmorButton.interactable = false;
//            Debug.Log("Reactive Armor Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Reactive Armor Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Second Wind------------------------------------------------------------------------------------------*/
//    private void OnSecondWindUpgrade()
//    {
//        if (playerCurrency >= secondWindCost)
//        {
//            playerCurrency -= secondWindCost;
//            secondWindUpgrade.isActive = true;
//            UpdateCurrencyDisplay();

//            secondWindButton.interactable = false;
//            Debug.Log("Second Wind Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Second Wind Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Speed Force------------------------------------------------------------------------------------------*/
//    private void OnSpeedForceUpgrade()
//    {
//        if (playerCurrency >= speedForceCost)
//        {
//            playerCurrency -= speedForceCost;
//            UpdateCurrencyDisplay();

//            if (speedForceUpgrade != null)
//            {
//                speedForceUpgrade.Activate();
//            }

//            speedForceButton.interactable = false;
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Speed Force Upgrade!");
//        }
//    }

//    private void ActivateSpeedForceUpgrade()
//    {
//        if (playerCurrency >= speedForceCost)
//        {
//            playerCurrency -= speedForceCost;
//            UpdateCurrencyDisplay();

//            if (speedForceUpgrade != null)
//            {
//                speedForceUpgrade.Activate();  // Make sure this method is defined in your SpeedForceUpgrade class
//            }

//            speedForceButton.interactable = false;
//            Debug.Log("Speed Force Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Speed Force Upgrade!");
//        }
//    }

//    /*----------------------------------------------------------------------------------------------------Temporal Echo------------------------------------------------------------------------------------------*/
//    private void OnTemporalEchoUpgrade()
//    {
//        if (playerCurrency >= temporalEchoCost)
//        {
//            playerCurrency -= temporalEchoCost;
//            UpdateCurrencyDisplay();

//            if (temporalEchoUpgrade != null)
//            {
//                temporalEchoUpgrade.Activate();
//            }

//            temporalEchoButton.interactable = false;
//            Debug.Log("Temporal Echo Upgrade Activated!");
//        }
//        else
//        {
//            Debug.Log("Not enough currency for Temporal Echo Upgrade!");
//        }
//    }
//}
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
