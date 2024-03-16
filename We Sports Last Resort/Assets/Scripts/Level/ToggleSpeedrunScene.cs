using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using WiiScripts.Input;

namespace Level
{
    public class ToggleSpeedrunScene : MonoBehaviour
    {
        [SerializeField] private string speedrunTimerField; 
        //For the future maybe use a library which gets all the strings by scene enums

        private async void OnEnable()
        {
            await Task.Yield();
            WiiMoteInput.Instance.OnButton_HOME += ProcessAction_OnButtonHome;
        }

        private void OnDisable()
        {
            WiiMoteInput.Instance.OnButton_HOME -= ProcessAction_OnButtonHome;
        }

        void ProcessAction_OnButtonHome(bool[] type)
        {
            
            
            if (type[1])
            {
                CoreEventManager.Instance.SceneEvents.OnToggleSceneToLoadOrUnload?.Invoke(speedrunTimerField);
            }
            
        }
    }
}

