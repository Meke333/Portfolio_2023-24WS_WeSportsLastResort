using System;
using Core;
using General.SingletonClass;
using UnityEngine;

namespace WiiScripts.Input
{
    public class WiiBalanceBoardInput : SingletonClass<WiiBalanceBoardInput>
    {
        #region Parameters

        public int balanceBoardDeviceNr;
        public bool isBalanceBoardActive;

        private Vector4 _weightDistribution;
        private Vector2 _centerOfBalance;
        private float _totalWeight;
        private float _rawTotalWeight;

        
        #endregion
        
        #region Events

        public event Action<bool> OnBalanceBoardActiveChange;
        public event Action<Vector4> OnWeightDistributionChange;
        public event Action<Vector2> OnCenterOfBalanceChange;
        public event Action<float> OnTotalWeightChange;

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            #region Observer

            //Debug.Log("IS COREWIIMANAGER == NULL: " + (CoreWiiManager.Instance == null));
            CoreWiiManager.Instance.onBalanceBoardDeviceChange += EvaluateAction_onBalanceBoardDeviceChange;

            #endregion
            
        }

        private void OnDisable()
        {
            #region Observer
            
            Debug.Log("IS COREWIIMANAGER == NULL: " + (CoreWiiManager.Instance == null));
            CoreWiiManager.Instance.onBalanceBoardDeviceChange -= EvaluateAction_onBalanceBoardDeviceChange;

            #endregion
        }

        private void Update()
        {
            if (!isBalanceBoardActive)
                return;

            ProcessTotalWeight();

            if (_rawTotalWeight < 0)
                return;
            
            ProcessWeightDistribution();
            ProcessCenterOfBalance();
        }

        #endregion

        #region Methods

        void ProcessWeightDistribution()
        {
            Vector4 temp = Wii.GetBalanceBoard(balanceBoardDeviceNr);
            if (_weightDistribution == temp)
                return;

            _weightDistribution = temp;
            OnWeightDistributionChange?.Invoke(_weightDistribution);
        }

        void ProcessCenterOfBalance()
        {
            Vector2 temp = Wii.GetCenterOfBalance(balanceBoardDeviceNr);
            if (_centerOfBalance == temp)
                return;

            _centerOfBalance = temp;
            OnCenterOfBalanceChange?.Invoke(_centerOfBalance);
        }

        void ProcessTotalWeight()
        {
            _rawTotalWeight = Wii.GetTotalWeight(balanceBoardDeviceNr);
            //if TotalWeight is lower than 0 return!!! or the difference is small then also return!
            if (_rawTotalWeight < 0 || Math.Abs(_totalWeight - _rawTotalWeight) < 0.01f)
                return;
            
            _totalWeight = _rawTotalWeight;
            OnTotalWeightChange?.Invoke(_totalWeight);
        }

        #endregion
        

        #region EventMethods
        

        
        //Observer
        void EvaluateAction_onBalanceBoardDeviceChange(int deviceNr)
        {
            balanceBoardDeviceNr = deviceNr;
            isBalanceBoardActive = balanceBoardDeviceNr >= 0;
            
            OnBalanceBoardActiveChange?.Invoke(isBalanceBoardActive);
        }


        #endregion
        
    }
}
