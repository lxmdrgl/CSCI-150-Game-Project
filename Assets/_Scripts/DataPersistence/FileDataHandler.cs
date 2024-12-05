using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public T Load<T>(string profileId) where T : class
    {
        if(profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);
        T loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);
                loadedData = JsonConvert.DeserializeObject<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save<T>(T data,string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);
            
            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
    public void Delete(string profileId)
    {
        if (profileId == null)
        {
            Debug.LogWarning("Profile ID is null. Cannot delete data.");
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath); // Only delete the GameData file
                Debug.Log($"Successfully deleted GameData file: {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred when trying to delete file: {fullPath}\n{e}");
            }
        }
        else
        {
            Debug.LogWarning($"No GameData file found to delete at: {fullPath}");
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
        
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            GameData profileData = Load<GameData>(profileId);

            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public Dictionary<string, SaveData> LoadAllProfilesSaveData()
    {
        Dictionary<string, SaveData> profileDictionary = new Dictionary<string, SaveData>();
        
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            SaveData profileData = Load<SaveData>(profileId);

            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecenProfileId = null;
        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if(mostRecenProfileId == null)
            {
                mostRecenProfileId = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecenProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                if(newDateTime > mostRecentDateTime)
                {
                    mostRecenProfileId = profileId;
                }
            }
        }
        return mostRecenProfileId;
    }
}
