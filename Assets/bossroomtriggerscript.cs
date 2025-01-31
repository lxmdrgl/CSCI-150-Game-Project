using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    // Event or action that gets triggered when the player enters the boss room
    public delegate void OnPlayerEnterRoom();
    public static event OnPlayerEnterRoom PlayerEnteredBossRoom;

    // Trigger when the player enters the boss room area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call the event or perform any other logic
            Debug.Log("Player has entered the boss room!");

            // Optionally, trigger an event or method to start the boss fight
            if (PlayerEnteredBossRoom != null)
            {
                PlayerEnteredBossRoom.Invoke();
            }
        }
    }

    // Optional: Trigger when the player exits the boss room
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has exited the boss room.");
        }
    }
}

public class BossFightManager : MonoBehaviour
{
    private void OnEnable()
    {
        BossRoomTrigger.PlayerEnteredBossRoom += StartBossFight;
    }

    private void OnDisable()
    {
        BossRoomTrigger.PlayerEnteredBossRoom -= StartBossFight;
    }

    private void StartBossFight()
    {
        // Example of what happens when the player enters the boss room
        Debug.Log("Boss fight has started!");
        // You can activate the boss, set up the health bar, etc.
    }
}