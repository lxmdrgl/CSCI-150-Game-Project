using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TextMeshProUGUI savesBtnText;
    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);

        Dictionary<string, SaveData> profilesSaveData = DataPersistenceManager.instance.GetAllProfilesSaveData();

        foreach(SaveSlot saveSlot in saveSlots)
        {
            SaveData profileData = null;
            profilesSaveData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        // Check if the profile has data, if not, create new data
        if (!DataPersistenceManager.instance.HasSaveData())
        {
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.NewSaveSlot();
            DataPersistenceManager.instance.SaveGame();
        }
    
        // Update UI or close menu as needed
        this.gameObject.SetActive(false);
        mainMenu.ActivateMenu();
        savesBtnText.text = "Save Slot: " + saveSlot.GetProfileId();
    }

    public void OnDeleteClicked(SaveSlot saveSlot)
    {
        string currentSave = DataPersistenceManager.instance.GetSelectedProfileId();

        DataPersistenceManager.instance.DeleteSaveSlotData(saveSlot.GetProfileId());
        ActivateMenu();

        if (saveSlot.GetProfileId() == currentSave)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

            int i = 0;
            while(!DataPersistenceManager.instance.HasGameData() && i<3)
            {
                Debug.Log("Checking Save: " + (i+1));
                DataPersistenceManager.instance.ChangeSelectedProfileId((i+1)+"");

                if(DataPersistenceManager.instance.HasGameData())
                {
                    savesBtnText.text = "Save Slot: " + (i+1)+"";
                    break;
                }
                else
                {
                    savesBtnText.text = "Save Slot:";
                }

                i++;
            }
        }
    }
}
