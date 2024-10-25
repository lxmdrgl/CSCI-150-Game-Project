using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Alex Gameplay Scene");
    }
    public void QuitGame()
    {
        Application.Quit();

    }
    public void GameSaves()
    {
        SceneManager.LoadScene("Save Data Scene");
    }
}
