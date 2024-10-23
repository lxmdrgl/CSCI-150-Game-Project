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

    public TMP_InputField usernameInput;  // Reference for username input field
    public TMP_InputField passwordInput;  // Reference for password input field
    public Text logTxt;  // Log text UI for feedback

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
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
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
            logTxt.text = "Account Signed In Successfully!";
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    // Sign Out
    public void SignOut()
    {
        logTxt.text = "Signing out...";

        try
        {
            AuthenticationService.Instance.SignOut();
            logTxt.text = "Signed out successfully!";
            Debug.Log("Player signed out successfully.");
            // Optionally, clear input fields or reset the UI here
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Sign out failed: {ex.Message}");
            logTxt.text = "Sign out failed!";
        }
    }
}
