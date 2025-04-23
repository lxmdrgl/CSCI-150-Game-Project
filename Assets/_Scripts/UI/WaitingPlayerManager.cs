using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WaitingPlayerManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public PlayerInputHandler InputHandler1 { get; private set; }
    public PlayerInputHandler InputHandler2 { get; private set; }
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;

    //private bool isPaused;
    
    [SerializeField] private GameObject singlePlayerScreenCanvasGO;
    [SerializeField] private GameObject foundPlayerScreenCanvasGO;
    [SerializeField] private GameObject menuFirst;
    [SerializeField] private GameObject menuFirst2;

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

    void Start()
    {
        SetDependencies();
        
        singlePlayerScreenCanvasGO.SetActive(false);
        foundPlayerScreenCanvasGO.SetActive(false);

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
    }


    public void Initialize()
    {
        //UnityEngine.Debug.LogWarning($"Calling WaitingPlayerManager.Initialize()");
        if (singlePlayerScreenCanvasGO != null)
        {
            singlePlayerScreenCanvasGO.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(menuFirst);

        Pause();
    }

    public void ExitScreen()
    {
        if (singlePlayerScreenCanvasGO != null)
        {
            singlePlayerScreenCanvasGO.SetActive(false);
        }
        // foundPlayerScreenCanvasGO.SetActive(true);

        // EventSystem.current.SetSelectedGameObject(menuFirst2);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Unpause();
    }

    public void SwitchScreen()
    {
        if (singlePlayerScreenCanvasGO != null)
        {
            singlePlayerScreenCanvasGO.SetActive(false);
        }
        if (foundPlayerScreenCanvasGO != null)
        {
            foundPlayerScreenCanvasGO.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(menuFirst2);

        Pause();
    }

    #region Pause/Unpause Functions
    public void Pause()
    {
        //isPaused = true;
        Time.timeScale = 0f;

        Debug.Log("Switching to UI");
        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("UI");
        }
        if (playerInput2 != null)
        {
            Debug.Log("Switching to UI Player 2");
            playerInput2.SwitchCurrentActionMap("UI");
        }
    }

    public void Unpause()
    {
        //isPaused = false;
        // foundPlayerScreenCanvasGO.SetActive(false);

        

        Time.timeScale = 1f;

        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("Player");
        }
        if (playerInput2 != null)
        {
            playerInput2.SwitchCurrentActionMap("Player");
        }

    }
    #endregion

    public void Singleplayer()
    {   
        ExitScreen();

        PlayerPrefs.SetFloat("runTime", 0f);
        PlayerPrefs.SetInt("playerCount",1);
        PlayerPrefs.SetInt("player1Kills", 0);
        PlayerPrefs.SetInt("player1Damage", 0);

        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            SceneManager.LoadScene(GameplaySceneName);
            singlePlayerScreenCanvasGO.SetActive(false);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            
            SceneManager.LoadScene(GameplaySceneName);
            singlePlayerScreenCanvasGO.SetActive(false);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();
        }
    }

    public void Quit()
    {   
        ExitScreen();

        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            SceneManager.LoadScene(MainMenuSceneName);
            singlePlayerScreenCanvasGO.SetActive(false);
            return;
        }
        else
        {
            DataPersistenceManager.instance.RestartGame( selectedProfileId, GameplaySceneName);
            

            SceneManager.LoadScene(MainMenuSceneName);
            singlePlayerScreenCanvasGO.SetActive(false);

            DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
            DataPersistenceManager.instance.SaveGame();   
        }
    }

    public void Resume()
    {   
        if (foundPlayerScreenCanvasGO != null)
        {
            foundPlayerScreenCanvasGO.SetActive(false);
        }
        // foundPlayerScreenCanvasGO.SetActive(true);

        // EventSystem.current.SetSelectedGameObject(menuFirst2);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Unpause();
    }
}
