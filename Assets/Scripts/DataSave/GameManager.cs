using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;  // Reference to the Player object

    private void Awake()
    {
        Load();
    }

    public void Save()
    {
        SaveSystem.SavePosition(player.Position);
        SaveSystem.SaveStats(player.playerStats); 
    }
    public void Load()
    {
        Vector3 loadedPosition = SaveSystem.LoadPosition();
        player.Position = loadedPosition;
        PlayerStats stats = SaveSystem.LoadStats();
        player.playerStats = stats;
    }

}
