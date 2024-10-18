using System.IO;
using UnityEngine;

public class PlayerDataSave : MonoBehaviour
{
    private string filePath;
    public TestData td;

    void Awake()
    {
        // Set the file path to store the JSON locally (persistentDataPath ensures it works across platforms)
        filePath = Path.Combine(Application.persistentDataPath, "TestData.json");
        SaveTestData(td);
    }

    // Save player data to a local JSON file
    public void SaveTestData(TestData data)
    {
        string json = JsonUtility.ToJson(data, true);  // Convert the object to a formatted JSON string
        File.WriteAllText(filePath, json);  // Write JSON to the file
        Debug.Log("Player data saved locally at: " + filePath);
    }

    // Load player data from the local JSON file
    public TestData LoadTestData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);  // Read JSON from the file
            TestData data = JsonUtility.FromJson<TestData>(json);  // Convert JSON back to an object
            Debug.Log("Player data loaded from local file.");
            return data;
        }
        else
        {
            Debug.LogWarning("No local save file found.");
            return null;
        }
    }
}
