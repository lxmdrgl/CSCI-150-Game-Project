using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(GameData data);

    void LoadSaveData(SaveData data);
    void SaveSaveData(SaveData data);
}   
