using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 50;    // ? set starting value in Inspector or here
    [SerializeField] private TextMeshProUGUI moneyText;

    private int currentMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentMoney = startingMoney;                   // ? actually assign it
        DontDestroyOnLoad(gameObject);
        UpdateUI();

        Debug.Log($"[CurrencyManager] Awake. startingMoney={startingMoney}, currentMoney={currentMoney}");
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
