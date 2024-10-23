using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(player, new Vector3(transform.position.x, -0.5f, 0), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
