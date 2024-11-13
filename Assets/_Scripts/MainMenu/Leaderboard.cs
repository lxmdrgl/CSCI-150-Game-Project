using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Leaderboards.Models;

public class Leaderboard : MonoBehaviour
{
    const string LeaderboardId = "Leaderboard";

    public GameObject leaderboardUI; // Assign the UI panel for the leaderboard in the inspector
    public TMP_Text leaderboardText; // Assign a Text or TMP_Text component to display scores

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    private async void Awake()
    { 
        // Subscribe to authentication events
        SubscribeToAuthenticationEvents();

        // Check if the user is already signed in
        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("User is signed in.");
            leaderboardUI.SetActive(true); // Show leaderboard UI
            await GetScores(); // Load scores
        }
        else
        {
            Debug.LogWarning("User is not signed in. Hiding leaderboard UI.");
            leaderboardUI.SetActive(false); // Hide leaderboard UI
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
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async Task GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

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
    }
}
