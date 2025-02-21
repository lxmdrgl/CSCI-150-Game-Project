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
        if(DataPersistenceManager.instance.disableDataPersistence == false)
        {
            selectedProfileId = DataPersistenceManager.instance.GetSelectedProfileId();
        }
    }


    public void DeathQuit()
    {   
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
