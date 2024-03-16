using System;
using Core;
using UnityEngine;

namespace UI
{
    public class UITutorial : MonoBehaviour
    {
        private Animator _currentAnimator;
        
        [SerializeField] private Animator movement;
        [SerializeField] private Animator attack;
        [SerializeField] private Animator blocking;
        [SerializeField] private Animator recentering;

        [SerializeField] private TutorialStage stage;

        private readonly int _startHash = Animator.StringToHash("Start");
        private readonly int _exitHash = Animator.StringToHash("Exit");

        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnTutorialStageChange += ProcessAction_OnTutorialStageChange;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnTutorialStageChange -= ProcessAction_OnTutorialStageChange;
        }

        void ProcessAction_OnTutorialStageChange(TutorialStage newStage)
        {
            if (_currentAnimator != null)
                _currentAnimator.SetTrigger(_exitHash);
            
            stage = newStage;

            switch (stage)
            {
                case TutorialStage.None:
                    _currentAnimator = null;
                    break;
                case TutorialStage.Movement:
                    _currentAnimator = movement;
                    break;
                case TutorialStage.Attacking:
                    _currentAnimator = attack;
                    break;
                case TutorialStage.Blocking:
                    _currentAnimator = blocking;
                    break;
                case TutorialStage.Recentering:
                    _currentAnimator = recentering;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_currentAnimator != null)
                _currentAnimator.SetTrigger(_startHash);
            
        }
    }

    public enum TutorialStage
    {
        None,
        Movement,
        Attacking,
        Blocking,
        Recentering,
    }
}
