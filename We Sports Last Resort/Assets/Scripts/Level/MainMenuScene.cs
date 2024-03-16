using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    private async void Start()
    {
        await Task.Yield();
        CoreEventManager.Instance.GameEvents.OnStartMenu?.Invoke();
    }
}
