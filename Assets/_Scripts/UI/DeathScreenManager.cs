using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenCanvasGO;

    private string selectedProfileId = "";

    public string MainMenuSceneName;

    public string GameplaySceneName;
    
    
    private void Awake()
    {
        foreach (var death in FindObjectsOfType<Game.CoreSystem.Death>())
        {
            death.OnDeath += DeathQuit;
        }
        
        if(DataPersistenceManager.instance.disableDataPersistence == false)
        {
            selectedProfileId = DataPersistenceManager.instance.GetSelectedProfileId();
        }
    }

    private void OnDestroy()
    {
        foreach (var death in FindObjectsOfType<Game.CoreSystem.Death>())
        {
            death.OnDeath -= DeathQuit;
        }
    }

    public void Start()
    {
    }


    public void DeathQuit()
    {   
        Time.timeScale = 1;
        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            SceneManager.LoadScene(MainMenuSceneName);
            deathScreenCanvasGO.SetActive(false);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            
            SceneManager.LoadScene(MainMenuSceneName);
            deathScreenCanvasGO.SetActive(false);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();
        }
    }

    public void DeathRestart()
    {   
        Time.timeScale = 1;
        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            SceneManager.LoadScene(GameplaySceneName);
            deathScreenCanvasGO.SetActive(false);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            

            SceneManager.LoadScene(GameplaySceneName);
            deathScreenCanvasGO.SetActive(false);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();   
        }
    }
}
