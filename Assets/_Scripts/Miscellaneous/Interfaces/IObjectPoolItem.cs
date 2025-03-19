using System;
using Game.ObjectPoolSystem;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IObjectPoolItem
    {
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        void Release();
    }
}