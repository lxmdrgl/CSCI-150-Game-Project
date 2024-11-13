using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class Authentication : MonoBehaviour
{
    public TMP_Text profileTxt;  
    public TMP_Text playerNameTxt; 
    public Button loginBtn;
    public Button signupBtn;
    public Button logOutBtn;
    public TMP_InputField usernameInput;  // Reference for username input field
    public TMP_InputField passwordInput;  // Reference for password input field
    public Text logTxt;  // Log text UI for feedback

    private async void Awake()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            AuthMenuSignedOut();
        }
        else
        {
            await LoadPlayerName();  // Fetch player name from Unity services
            AuthMenuSignedIn();
        }
    }

    private void AuthMenuSignedOut()
    {
        logOutBtn.gameObject.SetActive(false);      // dont show logout btn until logged in
        playerNameTxt.gameObject.SetActive(false);
        profileTxt.gameObject.SetActive(false);
        usernameInput.text = string.Empty;      // clear input fields and show login buttons/inputs
        passwordInput.text = string.Empty;
        usernameInput.gameObject.SetActive(true);
        passwordInput.gameObject.SetActive(true);
        loginBtn.gameObject.SetActive(true);
        signupBtn.gameObject.SetActive(true);
        logTxt.gameObject.SetActive(true);
    }
    private void AuthMenuSignedIn()
    {

        // Empty for now but add player name stats friends or something
        profileTxt.gameObject.SetActive(true);
        playerNameTxt.gameObject.SetActive(true);
        logOutBtn.gameObject.SetActive(true);
        usernameInput.gameObject.SetActive(false);
        passwordInput.gameObject.SetActive(false);
        loginBtn.gameObject.SetActive(false);
        signupBtn.gameObject.SetActive(false);
        logTxt.gameObject.SetActive(false);
    }

    // -------------- SIGN UP ---------------------
    public void SignUp() // FOR ONCLICKS, handles input and calls helper function to call sign in API
    {
        string username = usernameInput.text;  // Get the username
        string password = passwordInput.text;  // Get the password

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            logTxt.text = "Please enter both username and password!";
            return;
        }
        else
        {
            SignUpHelper(username, password);
        }
    }

    public async void SignUpHelper(string username, string password)  // Call Sign Up API
    {
        await SignUpWithUsernamePasswordAsync(username, password);
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password) // Sign Up API
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            logTxt.text = "Account Created And Signed In Successfully!";
            playerNameTxt.text = "Name: " + username;
            PlayerPrefs.SetString("PlayerName", username);  // Save locally
            Debug.Log("SignUp is successful.");
            AuthMenuSignedIn();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            logTxt.text = ex.Message;
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            logTxt.text = ex.Message;
            Debug.LogException(ex);
        }
    }

    // ----------- SIGN IN ---------------
    public void SignIn()    // FOR ONCLICKS, handles input and calls helper function to call sign in API
    {
        string username = usernameInput.text;  // Get the username
        string password = passwordInput.text;  // Get the password
        string playerName = usernameInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            logTxt.text = "Please enter both username and password!";
            return;
        }
        else
        {
            SignInHelper(username, password);
        }
    }

    public async void SignInHelper(string username, string password)  // Call Sign In API
    {
        await SignInWithUsernamePasswordAsync(username, password);
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)    // Sign In API
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
            string playerName = AuthenticationService.Instance.PlayerName;
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = username;
                PlayerPrefs.SetString("PlayerName", playerName);  // Save locally as a fallback
            }
            playerNameTxt.text = "Name: " + username;
            AuthMenuSignedIn();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            logTxt.text = ex.Message;
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            logTxt.text = ex.Message;
            Debug.LogException(ex);
        }
    }

    // Sign Out
    public void SignOut()
    {
        try
        {
            AuthenticationService.Instance.SignOut();
            PlayerPrefs.DeleteKey("PlayerName");  // Clear saved player name
            Debug.Log("Player signed out successfully.");
            AuthMenuSignedOut();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Sign out failed: {ex.Message}");
            logTxt.text = "Sign out failed!";
        }
    }

    private async Task LoadPlayerName()
    {
        try
        {
            // Get the player's name from Unity Authentication
            string username = AuthenticationService.Instance.PlayerName;

            if (string.IsNullOrEmpty(username))
            {
                username = PlayerPrefs.GetString("PlayerName", "Player");  // Fallback to local storage
            }

            playerNameTxt.text = "Name: " + username;
            PlayerPrefs.SetString("PlayerName", username);  // Ensure it's saved locally too
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to load player name: " + ex.Message);
            playerNameTxt.text = "Name: Unknown";
        }
    }

}
