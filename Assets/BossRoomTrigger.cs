using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject healthBar;

    // Trigger when the player enters the boss room area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // healthBar.SetActive(true);
        for (int i = 0; i < healthBar.transform.childCount; i++)
        {
            healthBar.transform.GetChild(i).gameObject.SetActive(true);
        }
        Destroy(gameObject);

    }

    public void Awake() 
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBar is not assigned.");
            return;
        }
        
        for (int i = 0; i < healthBar.transform.childCount; i++)
        {
            healthBar.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}