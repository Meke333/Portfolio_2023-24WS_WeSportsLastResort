using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Core;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    
    async void Start()
    {
        await Task.Delay(100);
        
        CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.Gameplay);
        MusicManager.Instance.PlayMusic(MusicManager.MusicType.Game);
    }

}
