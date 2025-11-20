using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenu : MonoBehaviour
{
    private UIDocument _document;
    private Button _upgradesButton;
    public PlayerHealth playerHealth;
    private ProgressBar healthBar;
    private ProgressBar cooldownBar;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        // Query buttons by their names in the UXML file
        _upgradesButton = _document.rootVisualElement.Q<Button>("Upgrades");

        if (_upgradesButton != null)
        {
            _upgradesButton.RegisterCallback<ClickEvent>(OnUpgradesClicked);
        }
        else
        {
            Debug.LogWarning("UpgradesButton not found in UIDocument!");
        }
    }

    private void OnUpgradesClicked(ClickEvent evt)
    {
        // add actual scene you want to load
        SceneManager.LoadScene("GameScene");
    }

    private void Start()
    {
        var root = _document.rootVisualElement;

        healthBar = root.Q<ProgressBar>("Health");

        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar not found in UIDocument!");
        }

        if (playerHealth != null)
        {
           // playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
        }
        else
        {
            Debug.LogWarning("PlayerHealth reference not set on InGameMenu!");
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void UpdateCooldownBar(float fillAmount)
    {
        if (cooldownBar != null)
            cooldownBar.value = Mathf.Clamp01(fillAmount);
    }
}
