using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class PlayerStats
{
    public float currentHp = 10;
    public float maxHp = 10;
    public string StatString() // for console debugging
    {
        return $"HP: {currentHp}/{maxHp}";
    }
}
