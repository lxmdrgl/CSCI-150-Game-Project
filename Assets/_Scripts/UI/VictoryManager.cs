using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public TMP_Text player1KillsText;
    public TMP_Text player1DamageText;
    public TMP_Text player2KillsText;
    public TMP_Text player2DamageText;
    public TMP_Text runTimeText;

    public void ShowVictoryScreen()
    {
        Time.timeScale = 0.3f; // Pause the game
        victoryScreen.SetActive(true);

        int p1Kills = PlayerPrefs.GetInt("player1Kills", 0);
        int p1Damage = PlayerPrefs.GetInt("player1Damage", 0);
        int p2Kills = PlayerPrefs.GetInt("player2Kills", 0);
        int p2Damage = PlayerPrefs.GetInt("player2Damage", 0);

        player1KillsText.text = $"Player 1 Kills: {p1Kills}";
        player1DamageText.text = $"Player 1 Damage: {p1Damage}";
        player2KillsText.text = $"Player 2 Kills: {p2Kills}";
        player2DamageText.text = $"Player 2 Damage: {p2Damage}";

        float totalTime =  PlayerPrefs.GetFloat("runTime") + Time.timeSinceLevelLoad; 
        TimeSpan totalTimeSpan = TimeSpan.FromSeconds(totalTime);
        string formattedTotalTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                totalTimeSpan.Hours,
                                                totalTimeSpan.Minutes,
                                                totalTimeSpan.Seconds);
        runTimeText.text += formattedTotalTime; 

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Adjust scene name if needed
    }
}
