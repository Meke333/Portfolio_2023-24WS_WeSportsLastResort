using System;
using PlayerScripts.Core;
using UnityEngine;
using WiiScripts.Input;

namespace PlayerScripts.PlayerSystemScripts
{
    [System.Serializable]
    public class PlayerProcessBalanceBoard
    {

        #region Parameter

        private PlayerScript _playerScript;
        
        //can be tweaked in Editor
        /*[Tooltip("This Variable will determine, when a step is triggered; " +
                 "\n You need to type in the difference in total Weight.")]
        [SerializeField] private float stepThreshhold = 20;*/

        [Tooltip("This Variable will determine, when the balance is on the other side, for a step")] [SerializeField] 
        private float stepThresholdWithCOG = 0.3f;
        
        [Tooltip("This Variable will determine, when a stretch from bend over position is triggered; " +
                 "\n You need to type in the difference in total Weight.")]
        [SerializeField] private float stretchThreshhold = 40;

        //code variables
        private float[] _totalWeightQueue = new float[10];
        private int _beginIndex = 0;
        private int _endIndex = 0;

        private int _stepSide = 0;

        private Vector2 _centerOfBalance;
        private Vector2 _balanceOffset;

        #endregion

        //Constructor
        public PlayerProcessBalanceBoard(PlayerScript newPlayerScript)
        {
            _playerScript = newPlayerScript;
        }
        
        #region Start & End Methods
        
        public void Initialize()
        {
            WiiBalanceBoardInput.Instance.OnTotalWeightChange += ProcessAction_OnTotalWeightChange;
            WiiBalanceBoardInput.Instance.OnWeightDistributionChange += ProcessAction_OnWeightDistributionChange;
            WiiBalanceBoardInput.Instance.OnCenterOfBalanceChange += ProcessAction_OnCenterOfBalanceChange;
        }

        public void ShutDown()
        {
            WiiBalanceBoardInput.Instance.OnTotalWeightChange -= ProcessAction_OnTotalWeightChange;
            WiiBalanceBoardInput.Instance.OnWeightDistributionChange -= ProcessAction_OnWeightDistributionChange;
            WiiBalanceBoardInput.Instance.OnCenterOfBalanceChange -= ProcessAction_OnCenterOfBalanceChange;
        }

        #endregion

        #region Methods

        void EstablishNeutralDenterOfBalance(float weight)
        {
            //Implement a check in game where the player has to stand still
            
        }

        #endregion

        #region EventMethods

        void ProcessAction_OnTotalWeightChange(float newWeight)
        {

            //Put it inside the _totalWeightQueue
            _totalWeightQueue[_beginIndex] = newWeight;
            
            //Only Track Weight if there are at least 10 values in queue
            if (_totalWeightQueue.Length < 10)
                return;
            
            //Check if the Weight currently is way higher than the the last one in the queue
            if (Mathf.Abs(_totalWeightQueue[_beginIndex] - _totalWeightQueue[_endIndex]) > stretchThreshhold)
            {
                _playerScript.PlayerEvents.onMovement_StretchingFromBentPosition?.Invoke();
                Debug.Log("BalanceBoard: STRETCHING!!!");
            }
            /*else if (Mathf.Abs(_totalWeightQueue[_beginIndex] - _totalWeightQueue[_endIndex]) > stepThreshhold)
            {
                _playerScript.PlayerEvents.onMovement_TakingAStep?.Invoke();
                Debug.Log("BalanceBoard: Taken a Step");
            }*/
            
            _beginIndex = (_beginIndex + 1) % 10;
            
            //if _beginIndex == _endIndex then endIndex will move and the next time event is triggered the field will be overwritten
            _endIndex += (_beginIndex == _endIndex) ? 1 : 0;
            _endIndex = _endIndex % 10;

        }

        void ProcessAction_OnWeightDistributionChange(Vector4 newDistribution)
        {
            
        }

        void ProcessAction_OnCenterOfBalanceChange(Vector2 newCenterOfBalance)
        {
            float centerOfBalanceAbs = Mathf.Abs(newCenterOfBalance.x);
            
            //if (centerOfBalanceAbs < 0.2)
            //    return;

            _centerOfBalance = newCenterOfBalance;
            
            _playerScript.PlayerEvents.onMovement_MovingToSides?.Invoke(newCenterOfBalance.x);

            #region Stepping

            if (centerOfBalanceAbs <= stepThresholdWithCOG)
                return;
            
            int tempSide = _stepSide;
                
            _stepSide = (_centerOfBalance.x > 0) ? 1 : -1 ;
            if (tempSide == _stepSide)
                return;

            _playerScript.PlayerEvents.onMovement_TakingAStep?.Invoke(_stepSide);

            #endregion

        }

        #endregion
        
        
    }
}
