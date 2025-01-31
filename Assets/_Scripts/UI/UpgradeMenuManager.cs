using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UpgradeMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private GameObject UpgradeCanvasGO;
    [SerializeField] private string fileName;

    private bool isPaused;

    public PlayerInputHandler InputHandler { get; private set; }
    private PlayerInput playerInput;

    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    
    
    private void Awake()
    {
        selectedProfileId = DataPersistenceManager.instance.GetSelectedProfileId();
    }

    void Start()
    {
        InputHandler = player.GetComponent<PlayerInputHandler>();
        playerInput = player.GetComponent<PlayerInput>();

        UpgradeCanvasGO.SetActive(false);

        Unpause();
    }

    void Update()
    {
        if (InputHandler.UpgradeOpenInput)
        {
            if (!isPaused)
            {
                Pause();
            }
        }/*
        else if (InputHandler.UIMenuCloseInput)
        {
            if (isPaused)
            {
                Unpause();
            }
        }*/
    }

    #region Pause/Unpause Functions
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        playerInput.SwitchCurrentActionMap("UI");

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        playerInput.SwitchCurrentActionMap("Player");

        CloseAllMenus();
    }
    #endregion

    #region Canvas Activations/Deactivations
    private void OpenMainMenu()
    {
        UpgradeCanvasGO.SetActive(true);

        InputHandler.UseUpgradeOpenInput();
    }

    private void CloseAllMenus()
    {
        UpgradeCanvasGO.SetActive(false);

        InputHandler.UseUpgradeOpenInput();
    }
    #endregion
}
