using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class SignInScript : MonoBehaviour
{
    public TMP_InputField usernameInput;  // Reference for username input field
    public TMP_InputField passwordInput;  // Reference for password input field
    public Text logTxt;  // Log text UI for feedback

    public void OnSignInButtonClicked()
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
            // Call your sign-up or authentication logic here
            SignIn(username, password);
        }
    }

    public async void SignIn(string username, string password)  // Sign in
    {
        await SignInWithUsernamePasswordAsync(username, password);
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
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

}
