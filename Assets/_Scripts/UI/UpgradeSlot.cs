using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Game.Weapons;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Id")]
    [SerializeField] private string upgradeId = "";

    [Header("Content")]
    private GameObject noDataContent;
    private GameObject hasDataContent;

    private StatUpgradeData currentData;
    // [SerializeField] private TextMeshProUGUI timePlayedText;

    public void Awake()
    {
        noDataContent = transform.Find("NoDataContent").gameObject;   
        hasDataContent = transform.Find("HasDataContent").gameObject;   
    }

    public void SetData(StatUpgradeData data)
    {
        if(data == null)
        {
            currentData = null;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            currentData = data;
            SetDataContent();
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
        }
    }
    public string GetProfileId()
    {
        return this.upgradeId;
    }

    public void SetDataContent()
    {
        // hasDataContent.transform.Find("UpgradeName").GetComponentInChildren<TextMeshProUGUI>().text = data.name;
        String text = "Attack: " + currentData.Attack.ToString() + ", Health: " + currentData.Health.ToString();
        transform.Find("Text (TMP)").GetComponentInChildren<TextMeshProUGUI>().text = currentData.name + "\n" + text;
    }
}
