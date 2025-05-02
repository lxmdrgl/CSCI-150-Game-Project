using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Core;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Authentication;



public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private GameObject mainMenuFirst;

    public string GameSceneName;
    public string generatorScene;

    private async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        Debug.Log("Loaded Player Name: " + playerName);  // Verify the name is loaded correctly

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void Play()
    {
        PlayerPrefs.SetFloat("runTime", 0f);
        PlayerPrefs.SetInt("playerCount",1);
        PlayerPrefs.SetInt("player1Kills", 0);
        PlayerPrefs.SetInt("player1Damage", 0);
        SceneManager.LoadScene(generatorScene);
    }

    public void PlayLocalMultiplayer()
    {
        PlayerPrefs.SetFloat("runTime", 0f);
        PlayerPrefs.SetInt("playerCount",2);
        PlayerPrefs.SetInt("player1Kills", 0);
        PlayerPrefs.SetInt("player1Damage", 0);
        PlayerPrefs.SetInt("player2Kills", 0);
        PlayerPrefs.SetInt("player2Damage", 0);
        SceneManager.LoadScene(generatorScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void ActivateMenu()
    {
        gameObject.SetActive(true);
        if(AuthenticationService.Instance.IsSignedIn)
        {
            handleLeaderBoard();
        }
    }
    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
        
        int p1Kills = PlayerPrefs.GetInt("player1Kills", 0);
        int p1Damage = PlayerPrefs.GetInt("player1Damage", 0);
        int p2Kills = PlayerPrefs.GetInt("player2Kills", 0);
        int p2Damage = PlayerPrefs.GetInt("player2Damage", 0);
        float runTime =  PlayerPrefs.GetFloat("runTime");
        int p1Score = (p1Kills * 100) + p1Damage - (int)runTime;
        int p2Score = (p2Kills * 100) + p2Damage - (int)runTime;
        int finalScore = Mathf.Max(p1Score, p2Score);

        if(finalScore > 0)
        {
            Leaderboard leaderboard = FindFirstObjectByType<Leaderboard>();
            if (leaderboard != null)
            {
                leaderboard.AddScore(finalScore);
                handleLeaderBoard();
            }
        }

        foreach (GameObject player in players)
        {
            if (player == null)
                continue;

            if (player.scene.IsValid() && player.scene.isLoaded)
            {
                Destroy(player);
            }
            else
            {
                Debug.LogWarning($"Skipping destruction of asset or invalid object: {player.name}");
            }
        }
    }

    private async void handleLeaderBoard()
    {
        Leaderboard leaderboard = FindFirstObjectByType<Leaderboard>();
        if (leaderboard != null)
        {
            leaderboard.SetLeaderboardPosition(false);
            await leaderboard.GetScores();
        }
    } 

    private void AddChildrenWithTag(Transform parent, string tag, List<GameObject> result)
    {
        if (parent.CompareTag(tag))
        {
            result.Add(parent.gameObject);
        }

        foreach (Transform child in parent)
        {
            AddChildrenWithTag(child, tag, result);
        }
    }
}
