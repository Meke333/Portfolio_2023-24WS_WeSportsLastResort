using System;
using UI;
using UnityEngine;

namespace Core.EventManagers
{
    public class GameEvents
    {
        public Action<TutorialStage> OnTutorialStageChange;
        public Action OnPlayerTakingAStep;
        public Action OnLevel1Finished;
        public Action OnPlayerDied;
        public Action OnLevelStarted;
        public Action OnStartMenu;
        public Action<Transform> OnTransmitPlayerPosition;

        public Action<bool> OnIsWiiMotionPlusActive;
        public Action<bool> OnIsWiiBalanceBoardActive;
        
        
    }
}
