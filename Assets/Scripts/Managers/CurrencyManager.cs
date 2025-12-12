using UnityEngine;
using TMPro;   // if you use TextMeshPro for UI

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int currentMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyText; // assign in Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    public int GetCoinCount()
    {
        return currentMoney;
    }

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + currentMoney;
        }
    }
}
