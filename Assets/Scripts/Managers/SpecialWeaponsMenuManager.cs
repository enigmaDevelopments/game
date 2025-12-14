using UnityEngine;
using UnityEngine.InputSystem;   // ? new Input System
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpecialWeaponsMenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject antennaboiMenu;
    [SerializeField] private GameObject seekerMenu;

    private AntennaboiSpecialMenu antennaboiMenuScript;
    private SeekerSpecialMenu seekerMenuScript;

    private GameObject currentMenu;
    private bool isOpen = false;

    private void Awake()
    {
        Debug.Log("SWMenuManager: Awake on " + gameObject.name);

        if (antennaboiMenu != null)
        {
            antennaboiMenuScript = antennaboiMenu.GetComponent<AntennaboiSpecialMenu>();
            antennaboiMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SWMenuManager: antennaboiMenu is not assigned.");
        }

        if (seekerMenu != null)
        {
            seekerMenuScript = seekerMenu.GetComponent<SeekerSpecialMenu>();
            seekerMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SWMenuManager: seekerMenu is not assigned.");
        }
    }

    private void Update()
    {
        // If there's no keyboard (very rare on PC), just skip
        if (Keyboard.current == null)
            return;

        // ? This is the correct way for Input System–only projects
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("SWMenuManager: Q pressed");
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        AssignCurrentMenu();  // check who is spawned right now

        if (currentMenu == null)
        {
            Debug.LogWarning("SWMenuManager: currentMenu is null, nothing to toggle.");
            return;
        }

        isOpen = !isOpen;
        currentMenu.SetActive(isOpen);
        Debug.Log($"SWMenuManager: Toggling {currentMenu.name} -> {(isOpen ? "OPEN" : "CLOSED")}");

        if (isOpen)
        {
            // ?? Select the first Button so keyboard/controller can navigate
            var firstButton = currentMenu.GetComponentInChildren<Button>();
            if (firstButton != null && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
                Debug.Log("SWMenuManager: Selected first button " + firstButton.name);
            }
        }
        else
        {
            // Clear selection when closing
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }


    private void AssignCurrentMenu()
    {
        // Look for the brains on the spawned player
        var seekerBrain = FindAnyObjectByType<SeekerBrain>();
        var antennaBrain = FindAnyObjectByType<AntennaBrain>();

        Debug.Log($"SWMenuManager: seekerBrain={(seekerBrain ? seekerBrain.gameObject.name : "null")}, " +
                  $"antennaBrain={(antennaBrain ? antennaBrain.gameObject.name : "null")}");

        // Prefer Seeker if present
        if (seekerBrain != null && seekerMenu != null && seekerMenuScript != null)
        {
            currentMenu = seekerMenu;
            seekerMenuScript.SetBrain(seekerBrain);
            Debug.Log("SWMenuManager: Using Seeker menu.");
            return;
        }

        // Otherwise use Antennaboi
        if (antennaBrain != null && antennaboiMenu != null && antennaboiMenuScript != null)
        {
            currentMenu = antennaboiMenu;
            antennaboiMenuScript.SetBrain(antennaBrain);
            Debug.Log("SWMenuManager: Using Antennaboi menu.");
            return;
        }

        currentMenu = null;
        Debug.LogWarning("SpecialWeaponsMenuManager: No Antennaboi or Seeker found in scene.");
    }
}
