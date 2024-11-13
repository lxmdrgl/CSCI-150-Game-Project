using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private GameObject mainMenuCanvasGO;
    [SerializeField] private GameObject optionsMenuCanvasGO;
    
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject optionsMenuFirst;

    private bool isPaused;

    public PlayerInputHandler InputHandler { get; private set; }
    private PlayerInput playerInput;

    void Start()
    {
        InputHandler = player.GetComponent<PlayerInputHandler>();
        playerInput = player.GetComponent<PlayerInput>();
        
        mainMenuCanvasGO.SetActive(false);
        optionsMenuCanvasGO.SetActive(false);

    }

    void Update()
    {
        if (InputHandler.MenuOpenInput)
        {
            if (!isPaused)
            {
                Pause();
            }
        }
        else if (InputHandler.UIMenuCloseInput)
        {
            if (isPaused)
            {
                Unpause();
            }
        }
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
        mainMenuCanvasGO.SetActive(true);
        optionsMenuCanvasGO.SetActive(false);

        InputHandler.UseMenuOpenInput();

        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    private void OpenOptionsMenuHandle()
    {
        mainMenuCanvasGO.SetActive(false);
        optionsMenuCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsMenuFirst);
    }

    private void CloseAllMenus()
    {
        mainMenuCanvasGO.SetActive(false);
        optionsMenuCanvasGO.SetActive(false);

        InputHandler.UseMenuOpenInput();

        EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region Main Menu Button Actions
    public void OnOptionsPress()
    {
        OpenOptionsMenuHandle();
    }

    public void OnContinuePress()
    {
        Unpause();
    }

    public void OnQuitPress()
    {
        // Placeholder, need to implement when merged with title screen UI. //
    }
    #endregion

    #region Options Menu Button Actions
    public void OnOptionsBackPress()
    {
        OpenMainMenu();
    }
    #endregion
}
