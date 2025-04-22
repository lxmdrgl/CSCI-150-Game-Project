using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public PlayerInputHandler InputHandler1 { get; private set; }
    public PlayerInputHandler InputHandler2 { get; private set; }
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;

    private bool isPaused;
    
    [SerializeField] private GameObject deathScreenCanvasGO;
    [SerializeField] private GameObject menuFirst;

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

    private void OnDestroy()
    {
        foreach (var death in FindObjectsOfType<Game.CoreSystem.Death>())
        {
            death.OnPlayerDeath -= Initialize;
        }
    }

    public void Start()
    {
        SetDependencies();
        
        deathScreenCanvasGO.SetActive(false);

        Unpause();
    }

    public void SetDependencies()
    {   
        if (player1 != null)
        {
            InputHandler1 = player1.GetComponent<PlayerInputHandler>();
            playerInput1 = player1.GetComponent<PlayerInput>();
        }
        if (player2 != null)
        {
            InputHandler2 = player2.GetComponent<PlayerInputHandler>();
            playerInput2 = player2.GetComponent<PlayerInput>();
        }

        foreach (var death in FindObjectsOfType<Game.CoreSystem.Death>())
        {
            death.OnPlayerDeath += Initialize;
        }
    }

    public void Initialize()
    {
        if (deathScreenCanvasGO != null)
        {
            deathScreenCanvasGO.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(menuFirst);

        Pause();
    }

    public void ExitScreen()
    {
        if (deathScreenCanvasGO != null)
        {
            deathScreenCanvasGO.SetActive(false);
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Unpause();
    }

    #region Pause/Unpause Functions
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.3f;
        /*
        Debug.Log("Switching to UI");
        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("UI");
        }
        if (playerInput2 != null)
        {
            Debug.Log("Switching to UI Player 2");
            playerInput2.SwitchCurrentActionMap("UI");
        }*/
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        /*
        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("Player");
        }
        if (playerInput2 != null)
        {
            playerInput2.SwitchCurrentActionMap("Player");
        }*/
    }
    #endregion

    public void DeathQuit()
    {   
        ExitScreen();
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
        ExitScreen();
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
