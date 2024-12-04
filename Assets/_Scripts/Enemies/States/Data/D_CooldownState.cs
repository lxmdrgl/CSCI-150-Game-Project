using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCooldownStateData", menuName = "Data/State Data/Cooldown State")]

public class D_CooldownState : ScriptableObject
{
    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 2f;
}
