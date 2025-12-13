using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialWeaponsMenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject antennaboiMenu;
    [SerializeField] private GameObject seekerMenu;

    private AntennaboiSpecialMenu antennaboiMenuScript;
    private SeekerSpecialMenu seekerMenuScript;

    private GameObject currentMenu;
    private bool isOpen = false;

    private PlayerInput input;
    private InputAction specialMenuAction;

    private void Awake()
    {
        input = FindFirstObjectByType<PlayerInput>();
        if (input == null)
        {
            Debug.LogError("SpecialWeaponsMenuManager: No PlayerInput found.");
            return;
        }

        specialMenuAction = input.actions.FindAction("SpecialWeaponsMenu");
        if (specialMenuAction == null)
        {
            Debug.LogError("SpecialWeaponsMenuManager: No 'SpecialWeaponsMenu' action in Input Actions.");
        }

        if (antennaboiMenu != null)
        {
            antennaboiMenuScript = antennaboiMenu.GetComponent<AntennaboiSpecialMenu>();
            antennaboiMenu.SetActive(false);
        }

        if (seekerMenu != null)
        {
            seekerMenuScript = seekerMenu.GetComponent<SeekerSpecialMenu>();
            seekerMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (specialMenuAction != null && specialMenuAction.WasPerformedThisFrame())
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        AssignCurrentMenu();  // check who is spawned right now

        if (currentMenu == null) return;

        isOpen = !isOpen;
        currentMenu.SetActive(isOpen);
    }

    private void AssignCurrentMenu()
    {
        // Is Antennaboi in the scene?
        var antennaBrain = FindAnyObjectByType<AntennaBrain>();
        if (antennaBrain != null && antennaboiMenu != null && antennaboiMenuScript != null)
        {
            currentMenu = antennaboiMenu;
            antennaboiMenuScript.SetBrain(antennaBrain);
            return;
        }

        // Is Seeker in the scene?
        var seekerBrain = FindAnyObjectByType<SeekerBrain>();
        if (seekerBrain != null && seekerMenu != null && seekerMenuScript != null)
        {
            currentMenu = seekerMenu;
            seekerMenuScript.SetBrain(seekerBrain);
            return;
        }

        currentMenu = null;
        Debug.LogWarning("SpecialWeaponsMenuManager: No Antennaboi or Seeker found in scene.");
    }
}
