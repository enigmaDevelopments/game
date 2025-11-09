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

    // Define costs and upgrade states
    public int criticalInstinctsCost = 3;  // Example cost for the Critical Instincts upgrade
    public int chainReactionCost = 3;      // Example cost for the Chain Reaction upgrade
    public int barrierSurgeCost = 3;

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
