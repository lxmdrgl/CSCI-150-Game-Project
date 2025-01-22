using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeSelector : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI timePlayedText;
    [SerializeField] private Button deleteBtn;

    public void SetData(SaveData data)
    {
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            deleteBtn.gameObject.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            deleteBtn.gameObject.SetActive(true);

            // Format playTime in hh:mm:ss
            TimeSpan timeSpan = TimeSpan.FromSeconds(data.playTime);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                 timeSpan.Hours,
                                                 timeSpan.Minutes,
                                                 timeSpan.Seconds);

            timePlayedText.text = "Time Played: " + formattedTime;
        }
    }
    public string GetProfileId()
    {
        return this.profileId;
    }
}
