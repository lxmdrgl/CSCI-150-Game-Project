using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject healthBar;

    // Trigger when the player enters the boss room area
    private void OnTriggerEnter2D(Collider2D other)
    {
        healthBar.SetActive(true);
        Destroy(gameObject);

    }

    public void Awake() {
        healthBar.SetActive(false);
    }
}