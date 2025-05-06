using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject bossCamera;
    public GameObject playerSpawn;
    public GameObject door;

    // Trigger when the player enters the boss room area
    private void OnTriggerEnter2D(Collider2D other)
    {   
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();

        foreach (GameObject player in players)
        {
            player.transform.position = playerSpawn.transform.position;
        }


        door.SetActive(true); // Open the door
        bossCamera.SetActive(true);
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
            // Debug.LogError("HealthBar is not assigned.");
            return;
        }
        
        for (int i = 0; i < healthBar.transform.childCount; i++)
        {
            healthBar.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}