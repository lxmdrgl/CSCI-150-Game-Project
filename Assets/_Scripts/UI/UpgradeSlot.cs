using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Game.Weapons;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    public StatUpgradeData currentStatData;
    public WeaponData currentWeaponData;

    public void Awake()
    {
        // noDataContent = transform.Find("NoDataContent").gameObject;   
        // hasDataContent = transform.Find("HasDataContent").gameObject;   
    }

    public void SetData(object data)
    {
        if (data == null)
        {
            currentStatData = null;
            currentWeaponData = null;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            if (data is StatUpgradeData statData)
            {
                currentStatData = statData;
                SetStatDataContent();
            }
            else if (data is WeaponData weaponData)
            {
                currentWeaponData = weaponData;
                SetWeaponDataContent();
            }

            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
        }
    }

    public void SetStatDataContent()
    {
        // hasDataContent.transform.Find("UpgradeName").GetComponentInChildren<TextMeshProUGUI>().text = data.name;
        String text = "Attack: " + currentStatData.Attack.ToString() + ", Health: " + currentStatData.Health.ToString();
        if (hasDataContent == null)
        {
            Debug.Log("hasDataContent is null");
        }
        hasDataContent.GetComponent<TextMeshProUGUI>().text = currentStatData.name + "\n" + text;
    }

    public void SetWeaponDataContent()
    {
        hasDataContent.GetComponent<TextMeshProUGUI>().text = currentWeaponData.Name + "\n" + currentWeaponData.Description;
    }
}
