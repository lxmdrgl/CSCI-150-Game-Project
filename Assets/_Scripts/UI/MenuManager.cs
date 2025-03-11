using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    
    [SerializeField] private GameObject mainMenuCanvasGO;
    [SerializeField] private GameObject optionsMenuCanvasGO;
    
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject optionsMenuFirst;

    private bool isPaused;

    public PlayerInputHandler InputHandler1 { get; private set; }
    public PlayerInputHandler InputHandler2 { get; private set; }
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;

    void Start()
    {

        SetDependencies();
        
        mainMenuCanvasGO.SetActive(false);
        optionsMenuCanvasGO.SetActive(false);

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

    void Update()
    {
        if ((InputHandler1 != null && InputHandler1.MenuOpenInput) || (InputHandler2 != null && InputHandler2.MenuOpenInput))
        {
            if (!isPaused)
            {
                Pause();
            }
        }
        else if ((InputHandler1 != null && InputHandler1.UIMenuCloseInput) || (InputHandler2 != null && InputHandler2.UIMenuCloseInput))
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

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("Player");
        }
        if (playerInput2 != null)
        {
            playerInput2.SwitchCurrentActionMap("Player");
        }

        CloseAllMenus();
    }
    #endregion

    #region Canvas Activations/Deactivations
    private void OpenMainMenu()
    {
        mainMenuCanvasGO.SetActive(true);
        optionsMenuCanvasGO.SetActive(false);

        if (InputHandler1 != null)
        {
            InputHandler1.UseMenuOpenInput();
        } 
        if (InputHandler2 != null)
        {
            InputHandler2.UseMenuOpenInput();
        }

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

        if (InputHandler1 != null)
        {
            InputHandler1.UseMenuOpenInput();
        }
        if (InputHandler2 != null)
        {
            InputHandler2.UseMenuOpenInput();
        }

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
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Options Menu Button Actions
    public void OnOptionsBackPress()
    {
        OpenMainMenu();
    }
    #endregion
}
