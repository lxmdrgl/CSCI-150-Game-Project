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
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    // [SerializeField] private TextMeshProUGUI timePlayedText;

    public void SetData(WeaponData data)
    {
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
        }
    }
    public string GetProfileId()
    {
        return this.upgradeId;
    }
}
