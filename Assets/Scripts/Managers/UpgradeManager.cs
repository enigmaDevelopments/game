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
    private CriticalInstinctsUpgrade criticalInstinctsUpgrade;
    public int criticalInstinctsCost = 3;

    public Button chainReactionButton;     // Button to activate Chain Reaction upgrade
    private ChainReactionUpgrade chainReactionUpgrade;
    public int chainReactionCost = 3;

    public Button barrierSurgeButton;
    private BarrierSurgeUpgrade barrierSurgeUpgrade;
    public int barrierSurgeCost = 3;

    public Button energyRecyclerButton;
    private EnergyRecyclerUpgrade energyRecyclerUpgrade;
    public int energyRecyclerCost = 200;

    public Button evasiveMomentumButton;
    private EvasiveMomentumUpgrade evasiveMomentumUpgrade;
    public int evasiveMomentumCost = 250;

    public Button guardianGraceButton;
    private GuardianGraceUpgrade guardianGraceUpgrade;
    public int guardianGraceCost = 300;

    public Button momentumButton;
    private MomentumUpgrade momentumUpgrade;
    public int momentumCost = 200;

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

    private int playerCurrency = 500;         // Player's current currency

    private void Start()
    {
        // Initialize UI elements
        UpdateCurrencyDisplay();
        UpdateUpgradeUI();

        // Add listeners to upgrade buttons
        criticalInstinctsButton.onClick.AddListener(OnCriticalInstinctsUpgrade);
        criticalInstinctsUpgrade = FindAnyObjectByType<CriticalInstinctsUpgrade>();
        chainReactionButton.onClick.AddListener(OnChainReactionUpgrade);
        chainReactionUpgrade = FindAnyObjectByType<ChainReactionUpgrade>();
        barrierSurgeButton.onClick.AddListener(OnBarrierSurgeUpgrade);
        barrierSurgeUpgrade = FindAnyObjectByType<BarrierSurgeUpgrade>();
        energyRecyclerButton.onClick.AddListener(OnEnergyRecyclerUpgrade);
        energyRecyclerUpgrade = FindAnyObjectByType<EnergyRecyclerUpgrade>();
        evasiveMomentumButton.onClick.AddListener(OnEvasiveMomentumUpgrade);
        evasiveMomentumUpgrade = FindAnyObjectByType<EvasiveMomentumUpgrade>();
        guardianGraceButton.onClick.AddListener(OnGuardianGraceUpgrade);
        guardianGraceUpgrade = FindAnyObjectByType<GuardianGraceUpgrade>();
        momentumButton.onClick.AddListener(OnMomentumUpgrade);
        momentumUpgrade = FindAnyObjectByType<MomentumUpgrade>();
        reactiveArmorButton.onClick.AddListener(OnReactiveArmorUpgrade);
        reactiveArmorUpgrade = FindAnyObjectByType<ReactiveArmorUpgrade>();
        secondWindButton.onClick.AddListener(OnSecondWindUpgrade);
        secondWindUpgrade = FindAnyObjectByType<SecondWindUpgrade>();
        speedForceButton.onClick.AddListener(OnSpeedForceUpgrade);
        speedForceUpgrade = FindAnyObjectByType<SpeedForceUpgrade>();
        temporalEchoButton.onClick.AddListener(OnTemporalEchoUpgrade);
        temporalEchoUpgrade = FindAnyObjectByType<TemporalEchoUpgrade>();
       
    }


    private void UpdateCurrencyDisplay()
    {
        // Update the player's current currency display
        playerCurrencyText.text = $"Currency: {playerCurrency}";
    }

    private void UpdateUpgradeUI()
    {
        // Update the UI based on the player's current currency and the cost of upgrades
        playerCurrencyText.text = $"Currency: {playerCurrency}";  // Update the currency display

        // List of buttons and their corresponding costs
        Button[] upgradeButtons = new Button[]
        {
        criticalInstinctsButton,
        chainReactionButton,
        barrierSurgeButton,
        energyRecyclerButton,
        evasiveMomentumButton,
        guardianGraceButton,
        momentumButton,
        reactiveArmorButton,
        secondWindButton,
        speedForceButton,
        temporalEchoButton,
        
        };

        int[] upgradeCosts = new int[]
        {
        criticalInstinctsCost,
        chainReactionCost,
        barrierSurgeCost,
        energyRecyclerCost,
        evasiveMomentumCost,
        guardianGraceCost,
        momentumCost,
        reactiveArmorCost,
        secondWindCost,
        speedForceCost,
        temporalEchoCost,
        
        };

        // Enable or disable buttons based on whether the player can afford the upgrade
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = playerCurrency >= upgradeCosts[i];
        }
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
    /*----------------------------------------------------------------------------------------------------critical instincts------------------------------------------------------------------------------------------*/
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
            Debug.Log("Not enough currency for Upgrade!");
        }
    }

    private void ActivateCriticalInstinctsUpgrade()
    {
        if (criticalInstinctsUpgrade != null)
        {
            // Activate the Critical Instincts upgrade
            criticalInstinctsUpgrade.ActivateCriticalInstincts(); // Call the method from CriticalInstinctsUpgrade

            // Update UI after activation
            criticalInstinctsButton.interactable = false;  // Disable the button after purchase
            Debug.Log("Critical Instincts Upgrade Activated!");
        }
        else
        {
            Debug.LogWarning("CriticalInstinctsUpgrade not found!");
        }
    }

    /*----------------------------------------------------------------------------------------------------Chain Reaction------------------------------------------------------------------------------------------*/
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
            Debug.Log("Not enough currency for Upgrade!");
        }
    }

    private void ActivateChainReactionUpgrade()
    {
        if (chainReactionUpgrade != null)
        {
            // Call the method to activate the chain reaction upgrade
            chainReactionUpgrade.ActivateChainReactionUpgrade();
            Debug.Log("Chain Reaction Upgrade Activated through Button!");
        }
        else
        {
            Debug.LogWarning("ChainReactionUpgrade not found!");
        }
    }

    /*----------------------------------------------------------------------------------------------------Barrier Surge------------------------------------------------------------------------------------------*/

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
            Debug.Log("Not enough currency for Upgrade!");
        }
    }

    private void ActivateBarrierSurgeUpgrade()
    {
        if(barrierSurgeUpgrade != null)
        {
            barrierSurgeUpgrade.ActivateBarrierSurge();
            Debug.Log("Barrier Surge Upgrade Activated");
        }
        else
        {
            Debug.LogWarning("Upgrade not found");
        }
    }

    /*----------------------------------------------------------------------------------------------------Energy Recycler------------------------------------------------------------------------------------------*/
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

    /*----------------------------------------------------------------------------------------------------Evasive Momentum------------------------------------------------------------------------------------------*/

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

    /*----------------------------------------------------------------------------------------------------Guardian Grace------------------------------------------------------------------------------------------*/
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

    /*----------------------------------------------------------------------------------------------------Momentum------------------------------------------------------------------------------------------*/
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

    /*----------------------------------------------------------------------------------------------------Reactive Armor------------------------------------------------------------------------------------------*/
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

    /*----------------------------------------------------------------------------------------------------Second Wind------------------------------------------------------------------------------------------*/
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

    /*----------------------------------------------------------------------------------------------------Speed Force------------------------------------------------------------------------------------------*/
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

    private void ActivateSpeedForceUpgrade()
    {
        if (playerCurrency >= speedForceCost)
        {
            playerCurrency -= speedForceCost;
            UpdateCurrencyDisplay();

            if (speedForceUpgrade != null)
            {
                speedForceUpgrade.Activate();  // Make sure this method is defined in your SpeedForceUpgrade class
            }

            speedForceButton.interactable = false;
            Debug.Log("Speed Force Upgrade Activated!");
        }
        else
        {
            Debug.Log("Not enough currency for Speed Force Upgrade!");
        }
    }

    /*----------------------------------------------------------------------------------------------------Temporal Echo------------------------------------------------------------------------------------------*/
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
}
