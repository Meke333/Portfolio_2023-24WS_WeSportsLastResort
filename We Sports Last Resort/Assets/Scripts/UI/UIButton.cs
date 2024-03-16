using System;
using Audio;
using Core;
using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Sprite pressedSprite;

        [SerializeField] private Image buttonImage;
        [SerializeField] private ButtonType buttonType;

        public void PointerInsideButton()
        {
            //Debug.LogWarning("ButtonEnter");
            buttonImage.sprite = selectedSprite;
        }

        public void PointerOutsideButton()
        {
            //Debug.LogWarning("ButtonExit");
            buttonImage.sprite = unselectedSprite;
        }

        public void PointerSelect()
        {
            //Debug.LogWarning("ButtonSelect");
            buttonImage.sprite = pressedSprite;
            Selected();

        }

        void Selected()
        {
            Debug.LogWarning("Selected Event Fired");
            switch (buttonType)
            {
                case ButtonType.None:
                    
                    break;
                case ButtonType.Menu_Start:
                    Debug.LogWarning("HELLO");
                    CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.Cutscene1);
                    MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                    break;
                case ButtonType.Menu_Exit:
                    CoreEventManager.Instance.SceneEvents.OnCloseGame?.Invoke();
                    break;
                case ButtonType.Gameplay_Restart:
                    CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.ResetLevel);
                    MusicManager.Instance.PlayMusic(MusicManager.MusicType.None, 0f);
                    break;
                case ButtonType.Gameplay_ReturnToMenu:
                    CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.MainMenu);
                    MusicManager.Instance.PlayMusic(MusicManager.MusicType.Theme);
                    break;
                case ButtonType.Gameplay_Finish:
                    CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.Cutscene3);
                    MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                    break;

                case ButtonType.Tutorial_Exit:
                    CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad?.Invoke(SceneSetups.Cutscene2);
                    MusicManager.Instance.PlayMusic(MusicManager.MusicType.None);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
        }
    }
    
    public enum ButtonType
    {
        None,
        Menu_Start,
        Menu_Exit,
        Tutorial_Exit,
        Gameplay_Restart,
        Gameplay_ReturnToMenu,
        Gameplay_Finish,
    }
}

