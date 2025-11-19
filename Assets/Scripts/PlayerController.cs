using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;

    // This might be called when switching characters or starting a new session
    public void SetPlayerData(PlayerData data)
    {
        playerData = data;

    }
}
