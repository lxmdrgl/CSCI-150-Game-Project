using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using Game.Weapons;
using Game.CoreSystem;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public float runTime;
    public float playerCurrentHp;
    public float playerMaxHp;
    public float currentLevelTime;
    public List<EnemyData> enemyData;
    public Vector2Data playerPosition; 
    public GameData()
    {
        this.runTime = 0;
        this.currentLevelTime = 0;
        this.playerCurrentHp = 100;
        this.playerMaxHp = 100;
        this.playerPosition = new Vector2Data(Vector2.zero);
        this.enemyData = new List<EnemyData>();
    }

    public struct EnemyData
    {
        public string UniqueId; 
        public Vector2Data Position;
        public float CurrentHp;
        public float MaxHp;
        public bool IsAlive;

        public EnemyData(string uniqueId, Vector2Data position, float currentHp, float maxHp, bool isAlive)
        {
            UniqueId = uniqueId;
            Position = position;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            IsAlive = isAlive;
        }
    }

    [Serializable]
    public class Vector2Data
    {
        public float x;
        public float y;

        public Vector2Data(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}
