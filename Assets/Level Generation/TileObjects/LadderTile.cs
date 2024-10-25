using UnityEngine;

public class LadderTile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collide Ladder tile" + " : " + gameObject.transform.position.x + ", " + gameObject.transform.position.y);
    }
}
