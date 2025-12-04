//Add all the target scenes (e.g. GameScene, LevelSelectScene, ShopScene) to File ? Build Settings ? Scenes in Build.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private UIDocument _document;
    private Button _startButton;
    private Button _selectLevelButton;
    private Button _shopButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        // Query buttons by their names in the UXML file
        _startButton = _document.rootVisualElement.Q<Button>("StartButton");
        _selectLevelButton = _document.rootVisualElement.Q<Button>("LevelSelect");
        _shopButton = _document.rootVisualElement.Q<Button>("ShopButton");

        // Register callbacks
        if (_startButton != null)
            _startButton.RegisterCallback<ClickEvent>(OnStartClicked);
        else
            Debug.LogWarning("StartButton not found in UIDocument!");

        if (_selectLevelButton != null)
            _selectLevelButton.RegisterCallback<ClickEvent>(OnSelectLevelClicked);
        else
            Debug.LogWarning("SelectLevelButton not found in UIDocument!");

        if (_shopButton != null)
            _shopButton.RegisterCallback<ClickEvent>(OnShopClicked);
        else
            Debug.LogWarning("ShopButton not found in UIDocument!");
    }

    private void OnStartClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("GameScene"); // Replace with your main game scene name
    }

    private void OnSelectLevelClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("LevelSelectScene"); // Replace with your level select scene name
    }

    private void OnShopClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("ShopScene"); // Replace with your shop scene name
    }
}
