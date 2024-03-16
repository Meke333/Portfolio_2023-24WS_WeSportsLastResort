using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;

public class GameplayLevel : MonoBehaviour
{
    private async void Start()
    {
        Task.Yield();
        CoreEventManager.Instance.GameEvents.OnLevelStarted?.Invoke();
    }
}
