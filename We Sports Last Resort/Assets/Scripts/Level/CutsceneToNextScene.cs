   using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Core;
using UnityEngine;
using UnityEngine.Video;
   using WiiScripts.Input;

   public class CutsceneToNextScene : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    [SerializeField] private SceneSetups nextSceneToLoad;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        _videoPlayer.loopPointReached += ProcessAction_LoopPointReached;
        WiiMoteInput.Instance.OnButton_MINUS += ProcessAction_OnButton_MINUS;
        WiiMoteInput.Instance.OnButton_ONE += ProcessAction_OnButton_MINUS;
    }

    private void OnDisable()
    {
        _videoPlayer.loopPointReached -= ProcessAction_LoopPointReached;
        WiiMoteInput.Instance.OnButton_MINUS -= ProcessAction_OnButton_MINUS;
        WiiMoteInput.Instance.OnButton_ONE -= ProcessAction_OnButton_MINUS;
    }

    #region EventMethods

    void ProcessAction_LoopPointReached(VideoPlayer source)
    {
        CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(nextSceneToLoad);

        switch (nextSceneToLoad)
        {
            case SceneSetups.MainMenu:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.Theme);
                break;
            case SceneSetups.Gameplay:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.Game);
                break;
            case SceneSetups.Cutscene1:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                break;
            case SceneSetups.Cutscene2:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                break;
            case SceneSetups.Cutscene3:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                break;
            case SceneSetups.ResetLevel:
                MusicManager.Instance.PlayMusic(MusicManager.MusicType.Game);
                break;
            case SceneSetups.TutorialLevel:
                break;
        }

    }
    
    
    void ProcessAction_OnButton_MINUS(bool[] a)
    {
        if (a[1])
        {
            ProcessAction_LoopPointReached(_videoPlayer);
        }
    }

    #endregion
    
    
}
