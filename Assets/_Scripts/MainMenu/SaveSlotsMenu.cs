using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TextMeshProUGUI savesBtnText;
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        // Check if the profile has data, if not, create new data
        if (!DataPersistenceManager.instance.HasGameData())
        {
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.SaveGame();
        }
    
        // Update UI or close menu as needed
        this.gameObject.SetActive(false);
        mainMenu.ActivateMenu();
        savesBtnText.text = "Save Slot: " + saveSlot.GetProfileId();
    }

    public void OnDeleteClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu();
    }
}
