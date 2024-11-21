using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI timePlayedText;
    [SerializeField] private Button deleteBtn;

    public void SetData(GameData data)
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

            timePlayedText.text = "Time Played: " + data.playTime;
        }
    }
    public string GetProfileId()
    {
        return this.profileId;
    }
}