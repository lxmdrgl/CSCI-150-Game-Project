using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using Unity.VisualScripting;

public class InputMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerInputCanvas;
    private bool isPaused;
    public GameObject player1;
    public GameObject player2;
    public PlayerInputHandler InputHandler1 { get; private set; }
    public PlayerInputHandler InputHandler2 { get; private set; }
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;
    public int playerIndex;
    [SerializeField] private GameObject inputMenuButton;

    public event Action<int> OnButtonClickedEvent;


    void Start()
    {
        SetDependencies();

        PlayerInputCanvas.SetActive(false);

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

    // Update is called once per frame
    void Update()
    {
        /* if ((InputHandler1 != null && InputHandler1.UpgradeOpenInput) || (InputHandler2 != null && InputHandler2.UpgradeOpenInput))
        {
            if (!isPaused)
            {
                Pause();
            }
        } */
    }
    public void OpenMenu()
    {
        Pause();
        playerInput1.enabled = false;
        playerInput2.enabled = false;
    }

    public void OnButtonClicked(int index)
    {
        Unpause();
        playerIndex = index;
        Debug.Log("Button clicked: " + index);
        playerInput1.enabled = true;
        playerInput2.enabled = true;
        OnButtonClickedEvent?.Invoke(index);
    }

    #region Pause/Unpause Functions
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (playerInput1 != null)
        {
            // playerInput1.SwitchCurrentActionMap("UI");
        }
        if (playerInput2 != null)
        {
            // playerInput2.SwitchCurrentActionMap("UI");
        }

        OpenMainMenu();
    }

    public void Unpause()
    {
        Debug.Log("Unpausing game...");
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
            PlayerInputCanvas.SetActive(true);

            /* if (InputHandler1 != null)
            {
                InputHandler1.UseUpgradeOpenInput();
            }
            if (InputHandler2 != null)
            {
                InputHandler2.UseUpgradeOpenInput();
            } */
            EventSystem.current.SetSelectedGameObject(inputMenuButton);
        }

        private void CloseAllMenus()
        {
            PlayerInputCanvas.SetActive(false);

            /* if (InputHandler1 != null)
            {
                InputHandler1.UseUpgradeOpenInput();
            }
            if (InputHandler2 != null)
            {
                InputHandler2.UseUpgradeOpenInput();
            } */

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null); // CLEAR SELECTED BUTTON FOR COLOR TRANSITIONS
            }
        }
        #endregion
}
