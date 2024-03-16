using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using WiiScripts.Input;

public class TutorialSkip : MonoBehaviour
{
    [SerializeField] private SceneSetups nextSceneToLoad;
    
    private void OnEnable()
    {
        //WiiMoteInput.Instance.OnButton_MINUS += SkipTutorial;
        //WiiMoteInput.Instance.OnButton_PLUS += SkipTutorial;
        WiiMoteInput.Instance.OnButton_ONE += SkipTutorial;
    }

    private void SkipTutorial(bool[] type)
    {
        if (type[1])
        {
            //Skip
            CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(nextSceneToLoad);

        }
    }
}
