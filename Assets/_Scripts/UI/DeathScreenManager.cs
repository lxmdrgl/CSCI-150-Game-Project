using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenCanvasGO;
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;
    private string selectedProfileId = "";

    public string MainMenuSceneName;

    public string GameplaySceneName;
    
    
    private void Awake()
    {
        selectedProfileId = DataPersistenceManager.instance.GetSelectedProfileId();
    }


    public void DeathQuit()
    {   
        DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
        deathScreenCanvasGO.SetActive(false);
        SceneManager.LoadScene(MainMenuSceneName);

        DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
        DataPersistenceManager.instance.SaveGame();
    }

    public void DeathRestart()
    {   
        DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
        deathScreenCanvasGO.SetActive(false);

        SceneManager.LoadScene(GameplaySceneName);

        DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
        DataPersistenceManager.instance.SaveGame();
    }
}
