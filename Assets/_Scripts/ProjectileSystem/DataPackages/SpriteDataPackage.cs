﻿using System;
using UnityEngine;

namespace Game.ProjectileSystem.DataPackages
{
    [Serializable]
    public class SpriteDataPackage : ProjectileDataPackage
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}