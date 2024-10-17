using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class SignOutScript : MonoBehaviour
{
    public Text logTxt;  // Log text UI for feedback

    // Method to sign out
    public void OnSignOutButtonClicked()
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