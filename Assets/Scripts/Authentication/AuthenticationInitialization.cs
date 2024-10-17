using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;

public class AuthenticationInitialization : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }
}
