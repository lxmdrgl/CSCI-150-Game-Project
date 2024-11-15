using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using Game.Weapons;
using Game.CoreSystem;

[System.Serializable]
public class GameData
{
    public float playTime;
    public float playerCurrentHp;
    public float playerMaxHp;
    public List<EnemyData> enemyData;
    public Vector2Data playerPosition; 
    public GameData()
    {
        this.playTime = 0;
        this.playerCurrentHp = 100;
        this.playerMaxHp = 100;
        this.playerPosition = new Vector2Data(Vector2.zero);
        this.enemyData = new List<EnemyData>();
    }

    public struct EnemyData
    {
        public string Guid;
        public bool isAlive;
        public float enemyCurrentHp;
        public float enemyMaxHp;
        public Vector2Data enemyPosition;

        public EnemyData(float enemyCurrentHp,float enemyMaxHp, Vector2Data enemyPosition,bool isAlive, string Guid)
        {
            this.enemyCurrentHp = enemyCurrentHp;
            this.enemyMaxHp = enemyMaxHp;
            this.enemyPosition = enemyPosition;
            this.isAlive = isAlive;
            this.Guid = Guid;
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
