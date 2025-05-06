using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

public class Leaderboard : MonoBehaviour
{
    const string LeaderboardId = "Leaderboard";
    public GameObject leaderboardUI; // Assign the UI panel for the leaderboard in the inspector
    public TMP_Text leaderboardText; // Assign a Text or TMP_Text component to display scores
    public RectTransform leaderboardPanel;
    public Vector3 mainMenuPosition;
    public Vector3 accountMenuPosition;
    private async void Awake()
    { 
        SceneManager.sceneLoaded += OnSceneLoadedAsync;

        await UnityServices.InitializeAsync();

        // Now it's safe to use AuthenticationService
        SubscribeToAuthenticationEvents();

        if (AuthenticationService.Instance.IsSignedIn)
        {
            leaderboardUI.SetActive(true);
            await GetScores();
        }
        else
        {
            leaderboardUI.SetActive(false);
        }
    }

    private void SubscribeToAuthenticationEvents()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignedOut += OnSignedOut;
    }

    private async void OnSignedIn()
    {
        leaderboardUI.SetActive(true); // Show the leaderboard UI when signed in
        //AddScore(420);
        await GetScores(); // Load the scores
    }

    private void OnSignedOut()
    {
        leaderboardUI.SetActive(false); // Hide the leaderboard UI when signed out
    }

    private async Task CheckUserSignInStatus()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            leaderboardUI.SetActive(true); // Show the leaderboard UI
            await GetScores(); // Load the scores
        }
        else
        {
            leaderboardUI.SetActive(false); // Hide the leaderboard UI
        }
    }

    public async void AddScore(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        // test Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async Task GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        // test Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        DisplayScores(scoresResponse);
    }

    private void DisplayScores(LeaderboardScoresPage scoresPage)
    {
        // Clear previous scores
        leaderboardText.text = "Leaderboard:\n";

        // Check if there are any scores to display
        if (scoresPage.Results.Count > 0)
        {
            // Iterate over the scores and display them
            foreach (var score in scoresPage.Results) // Access the results
            {
                leaderboardText.text += $"{score.PlayerName}: {score.Score}\n"; // Assuming PlayerId and Value exist
            }
        }
        else
        {
            leaderboardText.text += "No scores available.";
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
        AuthenticationService.Instance.SignedOut -= OnSignedOut;
        SceneManager.sceneLoaded -= OnSceneLoadedAsync;
    }

    private void OnSceneLoadedAsync(Scene scene, LoadSceneMode mode)
    {
        _ = HandleSceneLoadedAsync(scene, mode);
    }

    private async Task HandleSceneLoadedAsync(Scene scene, LoadSceneMode mode)
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            leaderboardUI.SetActive(true);
            await GetScores();
        }
        else
        {
            leaderboardUI.SetActive(false);
        }
    }

    public void SetLeaderboardPosition(bool inAccountMenu)
    {
        if (leaderboardPanel == null) return;

        leaderboardPanel.anchoredPosition = inAccountMenu ? accountMenuPosition : mainMenuPosition;
    }
}
