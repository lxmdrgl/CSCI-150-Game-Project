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
            deathScreenCanvasGO.SetActive(false);
            SceneManager.LoadScene(MainMenuSceneName);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            deathScreenCanvasGO.SetActive(false);
            SceneManager.LoadScene(MainMenuSceneName);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();
        }
    }

    public void DeathRestart()
    {   
        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            deathScreenCanvasGO.SetActive(false);
            SceneManager.LoadScene(GameplaySceneName);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            deathScreenCanvasGO.SetActive(false);

            SceneManager.LoadScene(GameplaySceneName);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();   
        }
    }
}
