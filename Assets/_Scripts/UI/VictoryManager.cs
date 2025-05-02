using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public TMP_Text player1KillsText;
    public TMP_Text player1DamageText;
    public TMP_Text player2KillsText;
    public TMP_Text player2DamageText;
    public TMP_Text runTimeText;
    public GameObject player1;
    public GameObject player2;
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;   
    public void ShowVictoryScreen()
    {
        Time.timeScale = 0.3f; // Pause the game
        victoryScreen.SetActive(true);

        if (player1 != null)
        {
            playerInput1 = player1.GetComponent<PlayerInput>();
            playerInput1.SwitchCurrentActionMap("UI");
        }
        if (player2 != null)
        {
            playerInput2 = player2.GetComponent<PlayerInput>();
            playerInput2.SwitchCurrentActionMap("UI");
        }

        int p1Kills = PlayerPrefs.GetInt("player1Kills", 0);
        int p1Damage = PlayerPrefs.GetInt("player1Damage", 0);
        int p2Kills = PlayerPrefs.GetInt("player2Kills", 0);
        int p2Damage = PlayerPrefs.GetInt("player2Damage", 0);
        
        if(playerInput1)
        {
            player1KillsText.text = $"Player 1 Kills: {p1Kills}";
            player1DamageText.text = $"Player 1 Damage: {p1Damage}";
        }
        if(playerInput2)
        {
            player2KillsText.text = $"Player 2 Kills: {p2Kills}";
            player2DamageText.text = $"Player 2 Damage: {p2Damage}";
        }

        float totalTime =  PlayerPrefs.GetFloat("runTime") + Time.timeSinceLevelLoad; 
        TimeSpan totalTimeSpan = TimeSpan.FromSeconds(totalTime);
        string formattedTotalTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                totalTimeSpan.Hours,
                                                totalTimeSpan.Minutes,
                                                totalTimeSpan.Seconds);
        runTimeText.text = $"Run Time: {formattedTotalTime}"; 

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Adjust scene name if needed
    }
}
