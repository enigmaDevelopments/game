using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    // Called when the Play / Start button is pressed
    public void StartGame()
    {
        // PlayerSelection.SelectedIndex already defaults to 0,
        // so we don't need to set anything here.

        Debug.Log("StartGame pressed. Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    // Optional: if you want a button to open the SelectPlayer menu
    public GameObject selectPlayerMenu;
    public GameObject startMenu;

    public void OpenSelectPlayerMenu()
    {
        startMenu.SetActive(false);
        selectPlayerMenu.SetActive(true);
    }

    // Optional: Back button
    public void BackToMainMenu()
    {
        startMenu.SetActive(true);
        selectPlayerMenu.SetActive(false);
    }
}
