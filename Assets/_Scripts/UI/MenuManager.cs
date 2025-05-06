using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Options Tabs")]
    [SerializeField] private GameObject volumePanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject volumeTabButton;
    [SerializeField] private GameObject gameplayTabButton;
    [SerializeField] private GameObject controlsTabButton;

    private GameObject currentTabButton;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI leaderboardText;

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

            InputHandler1.OnUIOptionMenuChanged += OnTabSelect;
        }
        if (player2 != null)
        {
            InputHandler2 = player2.GetComponent<PlayerInputHandler>();
            playerInput2 = player2.GetComponent<PlayerInput>();

            InputHandler2.OnUIOptionMenuChanged += OnTabSelect;
        }
    }

    public void OnDisable()
    {
        if (InputHandler1 != null)
        {
            InputHandler1.OnUIOptionMenuChanged -= OnTabSelect;
        }
        if (InputHandler2 != null)
        {
            InputHandler2.OnUIOptionMenuChanged -= OnTabSelect;
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

        // Debug.Log("Switching to UI");
        if (playerInput1 != null)
        {
            playerInput1.SwitchCurrentActionMap("UI");
        }
        if (playerInput2 != null)
        {
            // Debug.Log("Switching to UI Player 2");
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

        float totalTime =  PlayerPrefs.GetFloat("runTime") + Time.timeSinceLevelLoad; 
        float levelTime = Time.timeSinceLevelLoad;

        TimeSpan totalTimeSpan = TimeSpan.FromSeconds(totalTime);
        string formattedTotalTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                totalTimeSpan.Hours,
                                                totalTimeSpan.Minutes,
                                                totalTimeSpan.Seconds);
        TimeSpan levelTimeSpan = TimeSpan.FromSeconds(levelTime);
        string formattedLevelTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                levelTimeSpan.Hours,
                                                levelTimeSpan.Minutes,
                                                levelTimeSpan.Seconds);
        timeText.text = "Total run time: " + formattedTotalTime + "\n\n" + 
                        "Current level time: " + formattedLevelTime;

        leaderboardText.text = "Leaderboard: " + "\n\n";

        if (playerInput1)
        {
            leaderboardText.text += "Player 1: " + "\n" + 
                                    "  Kills: " + PlayerPrefs.GetInt("player1Kills") + "\n" + 
                                    "  Damage: " + PlayerPrefs.GetInt("player1Damage") + "\n\n"; 
        }
        if (playerInput2)
        {
            leaderboardText.text += "Player 2: " + "\n" + 
                                    "  Kills: " + PlayerPrefs.GetInt("player2Kills") + "\n" + 
                                    "  Damage: " + PlayerPrefs.GetInt("player2Damage") + "\n\n"; 
        }

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
        
        volumePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        controlsPanel.SetActive(false);

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

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
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
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Options Menu Button Actions
    public void OnOptionsBackPress()
    {
        OpenMainMenu();
    }

    public void ShowVolumePanel()
    {
        volumePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void ShowGameplayPanel()
    {
        volumePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ShowControlsPanel()
    {
        volumePanel.SetActive(false);
        gameplayPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    #endregion

    public void OnTabSelect()
    {
        // Debug.Log("Tab selected: " + EventSystem.current.currentSelectedGameObject.name);
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;

        if (selected == volumeTabButton)
        {
            ShowVolumePanel();
        }
        else if (selected == gameplayTabButton)
        {
            ShowGameplayPanel();
        }
        else if (selected == controlsTabButton)
        {
            ShowControlsPanel();
        }
    }
}
