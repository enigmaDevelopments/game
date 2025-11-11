using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenuPanel;  // Reference to the upgrade menu panel UI
    public Text upgradeLabel;            // Label for the menu title
    public Text playerCurrencyText;      // Text showing the player's current currency

    // Upgrade UI elements
    public Button criticalInstinctsButton;  // Button to activate Critical Instincts upgrade
    //public Text criticalInstinctsCostText;  // Text showing the cost for Critical Instincts

    public Button chainReactionButton;     // Button to activate Chain Reaction upgrade
    //public Text chainReactionCostText;     // Text showing the cost for Chain Reaction

    public Button barrierSurgeButton;
    //public Text barrierCostText;

    public Button energyRecyclerButton;
    private EnergyRecyclerUpgrade energyRecyclerUpgrade;

    public Button evasiveMomentumButton;
    private EvasiveMomentumUpgrade evasiveMomentumUpgrade;

    public Button guardianGraceButton;
    private GuardianGraceUpgrade guardianGraceUpgrade;

    public Button momentumButton;
    private MomentumUpgrade momentumUpgrade;

    public Button reactiveArmorButton;
    private ReactiveArmorUpgrade reactiveArmorUpgrade;
    public int reactiveArmorCost = 250;

    public Button secondWindButton;
    public int secondWindCost = 400;
    private SecondWindUpgrade secondWindUpgrade;

    public Button speedForceButton;
    public int speedForceCost = 300;
    private SpeedForceUpgrade speedForceUpgrade;

    public Button temporalEchoButton;
    public int temporalEchoCost = 500;
    private TemporalEchoUpgrade temporalEchoUpgrade;

    // Define costs and upgrade states
    public int criticalInstinctsCost = 3;  // Example cost for the Critical Instincts upgrade
    public int chainReactionCost = 3;      // Example cost for the Chain Reaction upgrade
    public int barrierSurgeCost = 3;
    public int energyRecyclerCost = 200;
    public int evasiveMomentumCost = 250;
    public int guardianGraceCost = 300;
    public int momentumCost = 200;

    private int playerCurrency = 500;         // Player's current currency

    private void Start()
    {
        // Initialize UI elements
        UpdateCurrencyDisplay();
        UpdateUpgradeUI();

        // Add listeners to upgrade buttons
        criticalInstinctsButton.onClick.AddListener(OnCriticalInstinctsUpgrade);
        chainReactionButton.onClick.AddListener(OnChainReactionUpgrade);
        barrierSurgeButton.onClick.AddListener(OnBarrierSurgeUpgrade);
        energyRecyclerButton.onClick.AddListener(OnEnergyRecyclerUpgrade);
        evasiveMomentumButton.onClick.AddListener(OnEvasiveMomentumUpgrade);
        evasiveMomentumUpgrade = FindFirstObjectByType<EvasiveMomentumUpgrade>();
        guardianGraceButton.onClick.AddListener(OnGuardianGraceUpgrade);
        guardianGraceUpgrade = FindFirstObjectByType<GuardianGraceUpgrade>();
        momentumButton.onClick.AddListener(OnMomentumUpgrade);
        momentumUpgrade = FindFirstObjectByType<MomentumUpgrade>();
        reactiveArmorButton.onClick.AddListener(OnReactiveArmorUpgrade);
        reactiveArmorUpgrade = FindFirstObjectByType<ReactiveArmorUpgrade>();
        secondWindButton.onClick.AddListener(OnSecondWindUpgrade);
        secondWindUpgrade = FindFirstObjectByType<SecondWindUpgrade>();
        speedForceButton.onClick.AddListener(OnSpeedForceUpgrade);
        speedForceUpgrade = FindFirstObjectByType<SpeedForceUpgrade>();
        temporalEchoButton.onClick.AddListener(OnTemporalEchoUpgrade);
        temporalEchoUpgrade = FindFirstObjectByType<TemporalEchoUpgrade>();
        energyRecyclerButton.onClick.AddListener(OnEnergyRecyclerUpgrade);









        energyRecyclerUpgrade = FindFirstObjectByType<EnergyRecyclerUpgrade>();
    }

    private void UpdateCurrencyDisplay()
    {
        // Update the player's current currency display
        playerCurrencyText.text = $"Currency: {playerCurrency}";
    }

    private void UpdateUpgradeUI()
    {
        // Update the UI based on the player's current currency and the cost of upgrades
        //criticalInstinctsCostText.text = $"Cost: {criticalInstinctsCost}";
        //chainReactionCostText.text = $"Cost: {chainReactionCost}";

        // Enable or disable buttons based on whether the player can afford the upgrade
        criticalInstinctsButton.interactable = playerCurrency >= criticalInstinctsCost;
        chainReactionButton.interactable = playerCurrency >= chainReactionCost;
    }

    public void ShowUpgradeMenu()
    {
        // Show the upgrade menu panel
        upgradeMenuPanel.SetActive(true);
        UpdateCurrencyDisplay();
        UpdateUpgradeUI();
    }

    public void CloseUpgradeMenu()
    {
        // Close the upgrade menu panel
        upgradeMenuPanel.SetActive(false);
    }

    private void OnCriticalInstinctsUpgrade()
    {
        if (playerCurrency >= criticalInstinctsCost)
        {
            // Deduct the cost and apply the upgrade
            playerCurrency -= criticalInstinctsCost;
            ActivateCriticalInstinctsUpgrade();
            UpdateCurrencyDisplay();
            UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough currency for Critical Instincts Upgrade!");
        }
    }

    private void OnChainReactionUpgrade()
    {
        if (playerCurrency >= chainReactionCost)
        {
            // Deduct the cost and apply the upgrade
            playerCurrency -= chainReactionCost;
            ActivateChainReactionUpgrade();
            UpdateCurrencyDisplay();
            UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough currency for Chain Reaction Upgrade!");
        }
    }

    private void OnBarrierSurgeUpgrade()
    {
        if (playerCurrency >= barrierSurgeCost)
        {
            // Deduct the cost and apply the upgrade
            playerCurrency -= barrierSurgeCost;
            ActivateBarrierSurgeUpgrade();
            UpdateCurrencyDisplay();
            UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough currency for Chain Reaction Upgrade!");
        }
    }

    private void OnEnergyRecyclerUpgrade()
    {
        if (playerCurrency >= energyRecyclerCost)
        {
            playerCurrency -= energyRecyclerCost;
            energyRecyclerUpgrade.isActive = true;
            UpdateCurrencyDisplay();
            Debug.Log("Energy Recycler Upgrade Activated!");
            energyRecyclerButton.interactable = false; // disable after buying
        }
        else
        {
            Debug.Log("Not enough currency for Energy Recycler Upgrade!");
        }
    }

    private void OnEvasiveMomentumUpgrade()
    {
        if (playerCurrency >= evasiveMomentumCost)
        {
            playerCurrency -= evasiveMomentumCost;
            evasiveMomentumUpgrade.isActive = true;
            UpdateCurrencyDisplay();

            evasiveMomentumButton.interactable = false;
            Debug.Log("Evasive Momentum Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Evasive Momentum Upgrade!");
        }
    }

    private void OnGuardianGraceUpgrade()
    {
        if (playerCurrency >= guardianGraceCost)
        {
            playerCurrency -= guardianGraceCost;
            guardianGraceUpgrade.isActive = true;
            UpdateCurrencyDisplay();

            guardianGraceButton.interactable = false;
            Debug.Log("Guardian Grace Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Guardian Grace Upgrade!");
        }
    }

    private void OnMomentumUpgrade()
    {
        if (playerCurrency >= momentumCost)
        {
            playerCurrency -= momentumCost;
            momentumUpgrade.isActive = true;
            UpdateCurrencyDisplay();

            momentumButton.interactable = false;
            Debug.Log("Momentum Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Momentum Upgrade!");
        }
    }

    private void OnReactiveArmorUpgrade()
    {
        if (playerCurrency >= reactiveArmorCost)
        {
            playerCurrency -= reactiveArmorCost;
            reactiveArmorUpgrade.isActive = true;
            UpdateCurrencyDisplay();

            reactiveArmorButton.interactable = false;
            Debug.Log("Reactive Armor Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Reactive Armor Upgrade!");
        }
    }

    private void OnSecondWindUpgrade()
    {
        if (playerCurrency >= secondWindCost)
        {
            playerCurrency -= secondWindCost;
            secondWindUpgrade.isActive = true;
            UpdateCurrencyDisplay();

            secondWindButton.interactable = false;
            Debug.Log("Second Wind Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Second Wind Upgrade!");
        }
    }

    private void OnSpeedForceUpgrade()
    {
        if (playerCurrency >= speedForceCost)
        {
            playerCurrency -= speedForceCost;
            UpdateCurrencyDisplay();

            if (speedForceUpgrade != null)
            {
                speedForceUpgrade.Activate();
            }

            speedForceButton.interactable = false;
        }
        else
        {
            Debug.Log("Not enough currency for Speed Force Upgrade!");
        }
    }

    private void OnTemporalEchoUpgrade()
    {
        if (playerCurrency >= temporalEchoCost)
        {
            playerCurrency -= temporalEchoCost;
            UpdateCurrencyDisplay();

            if (temporalEchoUpgrade != null)
            {
                temporalEchoUpgrade.Activate();
            }

            temporalEchoButton.interactable = false;
            Debug.Log("Temporal Echo Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Temporal Echo Upgrade!");
        }
    }

    private void ActivateCriticalInstinctsUpgrade()
    {
        // Code to activate the Critical Instincts upgrade
        // (For example, you can call a method in the CriticalInstinctsUpgrade script)
        Debug.Log("Critical Instincts Upgrade Activated!");
        // Here, you would activate the Critical Instincts upgrade in your game logic.
    }

    private void ActivateChainReactionUpgrade()
    {
        // Code to activate the Chain Reaction upgrade
        // (For example, you can call a method in the ChainReactionUpgrade script)
        Debug.Log("Chain Reaction Upgrade Activated!");
        // Here, you would activate the Chain Reaction upgrade in your game logic.
    }

    private void ActivateBarrierSurgeUpgrade()
    {
        Debug.Log("Barrier Surge Upgrade Activated");
    }
}
