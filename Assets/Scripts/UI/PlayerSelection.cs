using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    // Index of the chosen player prefab
    // 0 = default if nothing was selected
    public static int SelectedIndex { get; private set; } = 0;
    public static bool HasSelection { get; private set; } = false;

    // Call this from each character button, with its index
    public void ChoosePlayer(int index)
    {
        SelectedIndex = index;
        HasSelection = true;
        Debug.Log("PlayerSelection: chose index " + index);
    }
}
