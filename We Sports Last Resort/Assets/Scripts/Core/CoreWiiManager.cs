using System;
using System.Threading.Tasks;
using General.SingletonClass;
using UnityEngine;
using WiiScripts.Connection;

namespace Core
{
    public class CoreWiiManager : SingletonClass<CoreWiiManager>
    {
        #region Parameters

        [SerializeField] private int totalDevices;
        [SerializeField] private int wiiMotionPlusDevice = -1;
        [SerializeField] private int balanceBoardDevice = -1;

        private bool _isSetupFinished;
        private bool _isMotionPlusCalibrated;
        private float _currentTimeToWait;
        private float _checkTime = 1f;
        private float _calibrateTime = 3f;
        private bool _checkIsForNowOver;
        private bool _calibrationIsForNowOver;

        #region Properties

        public int TotalDevices
        {
            get => totalDevices;
            set
            {
                totalDevices = value;
                
                //Fire Methods or Events here
                onTotalDevicesChange?.Invoke(totalDevices);
            }
        }

        public int WiiMotionPlusDevice
        {
            get => wiiMotionPlusDevice;
            set
            {
                wiiMotionPlusDevice = value;
                
                //FireMethods or Events here
                onWiiMotionPlusDeviceChange?.Invoke(wiiMotionPlusDevice);
            }
        }
        
        public int BalanceBoardDevice
        {
            get => balanceBoardDevice;
            set
            {
                balanceBoardDevice = value;
                
                //FireMethods or Events here
                onBalanceBoardDeviceChange?.Invoke(balanceBoardDevice);
            }
        }

        #endregion

        private WiiConnection _wiiConnection;
        

        #endregion

        #region Events

        public event Action<int> onTotalDevicesChange; 
        public event Action<int> onWiiMotionPlusDeviceChange;
        public event Action<int> onBalanceBoardDeviceChange;

        #endregion

        #region UnityMethods

        public override void Awake()
        {
            base.Awake();

            WiiMotionPlusDevice = -1;
            BalanceBoardDevice = -1;

            _wiiConnection = new WiiConnection();
            
           //Debug.Log("COREWIIAWAKE IS FINISHED!!!");
        }

        private async void OnEnable()
        {
            #region Observer

            _wiiConnection.onTotalDeviceChange += value => TotalDevices = value;
            _wiiConnection.onWiiMotionPlusDeviceChange += value => WiiMotionPlusDevice = value;
            _wiiConnection.onBalanceBoardDeviceChange += value => BalanceBoardDevice = value;

            #endregion

            await Task.Delay(300);
            
            _wiiConnection.Initializing();
            
            await Task.Delay(1000);
            
            _wiiConnection.BeginSearch();
            Debug.LogWarning("ok1");
            
            await Task.Delay(2000);
            
            _wiiConnection.BeginSearch();
            Debug.LogWarning("ok2");
            
            await Task.Delay(2000);
            
            _wiiConnection.BeginSearch();
            Debug.LogWarning("ok3");
            
            _wiiConnection.BeginSearch();
            Debug.LogWarning("ok1");
            
            _wiiConnection.BeginSearch();
            Debug.LogWarning("ok1");
            
            await Task.Delay(1000);

            _isSetupFinished = true;
            
            do
            {
                Debug.LogWarning("CHECKFORMOTIONPLUS");
                _wiiConnection.CheckForMotionPlus();
                
                await Task.Delay(1500); 
                Debug.LogWarning("CALIBRATEMOTIONPLUS"); 
                _wiiConnection.CalibrateMotionPlus();
                await Task.Delay(1500); 
            } while (!_wiiConnection.IsWiiMotionCalibrated());
            
            
            
            CoreEventManager.Instance.GameEvents.OnIsWiiMotionPlusActive?.Invoke(true);
            
            await Task.Delay(500); 
            
            CoreEventManager.Instance.GameEvents.OnIsWiiBalanceBoardActive?.Invoke(_wiiConnection.IsWiiBalanceBoardActive());
        }

        private void OnDisable()
        {
            #region Observer

            _wiiConnection.onTotalDeviceChange -= value => TotalDevices = value;
            _wiiConnection.onWiiMotionPlusDeviceChange -= value => WiiMotionPlusDevice = value;
            _wiiConnection.onBalanceBoardDeviceChange -= value => BalanceBoardDevice = value;

            #endregion
            
            _wiiConnection.ShutDown();
        }

        /*private void LateUpdate()
        {
            if (!_isSetupFinished)
                return;
            
            if (_isMotionPlusCalibrated)
                return;

            _currentTimeToWait += Time.unscaledDeltaTime;

            if (_currentTimeToWait < _checkTime)
                return;

            if (!_checkIsForNowOver)
            {
                _checkIsForNowOver = true;
            
                _wiiConnection.CheckForMotionPlus();
            }
            
            if (_currentTimeToWait < _calibrateTime)
                return;
             
            Debug.Log("1");
            _wiiConnection.CalibrateMotionPlus();

            if (_wiiConnection.IsWiiMotionCalibrated())
            {
                _isMotionPlusCalibrated = true;
                
                CoreEventManager.Instance.GameEvents.OnIsWiiMotionPlusActive?.Invoke(true);
                CoreEventManager.Instance.GameEvents.OnIsWiiBalanceBoardActive?.Invoke(_wiiConnection.IsWiiBalanceBoardActive());

            }

            _checkIsForNowOver = false;
            _currentTimeToWait = 0;

        }*/

        #endregion
    }
}
