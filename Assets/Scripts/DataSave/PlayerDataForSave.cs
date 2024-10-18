using UnityEngine;
using System;

public class PlayerDataForSave : MonoBehaviour
{
    public TestData testData = new TestData();
}

[Serializable]  // Allows the class to be converted to JSON
public class TestData
{
    public string playerName;
    public int level;
    public int coins;

}
