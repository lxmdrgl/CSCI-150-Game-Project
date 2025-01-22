using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/State Data/Player Detected State")]
public class D_PlayerDetected : ScriptableObject
{
    public float movementSpeed = 3f; // Added movement speed to fix the error

    public float detectedTime = .25f;
}
