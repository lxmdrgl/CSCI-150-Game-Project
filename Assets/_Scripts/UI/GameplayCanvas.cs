using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    public GameObject playerHealthBar1;
    public GameObject playerHealthBar2;

    void Start()
    {
        SetDependencies();
    }

    public void SetDependencies()
    {
        if (playerHealthBar1 != null)
        {
            playerHealthBar1.GetComponent<HealthBar>().SetDependencies();
        }
        if (playerHealthBar2 != null)
        {
            playerHealthBar2.GetComponent<HealthBar>().SetDependencies();
        }
    }
    void Update()
    {
        
    }
}
