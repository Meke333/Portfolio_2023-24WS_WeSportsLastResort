
using System;
using System.Threading.Tasks;
using Core;
using UnityEngine;

namespace WiiScripts.Connection
{
    public class WiiConnection
    {
        int currentDevice;

        private int totalDevices;
        private int wiiMotionPlusDeviceNr;
        private bool motionPlusDetected;
        private int balanceBoardDeviceNr;
        private bool balanceBoardDetected;

        public int TotalDevices
        {
            get => totalDevices;
            set
            {
                totalDevices = value;
                onTotalDeviceChange?.Invoke(totalDevices);
            }
        }

        public int WiiMotionPlusDeviceNr
        {
            get => wiiMotionPlusDeviceNr;
            set
            {
                wiiMotionPlusDeviceNr = value;
                onWiiMotionPlusDeviceChange?.Invoke(wiiMotionPlusDeviceNr);
            }
        }

        public int BalanceBoardDeviceNr
        {
            get => balanceBoardDeviceNr;
            set
            {
                balanceBoardDeviceNr = value;
                onBalanceBoardDeviceChange?.Invoke(balanceBoardDeviceNr);
            }
        }

        public bool isWiiMotionPlusActive { get; private set; }
        public bool isBalanceBoardActive { get; private set; }

        
        #region Event

        public Action<int> onTotalDeviceChange;
        public Action<int> onWiiMotionPlusDeviceChange;
        public Action<int> onBalanceBoardDeviceChange;

        #endregion
        
        public void Initializing()
        {
            
            Wii.ShouldAutomaticallyCalibrateMotionPlus(true);
            
            Wii.OnDiscoveryFailed     += OnDiscoveryFailed;
            Wii.OnWiimoteDiscovered   += OnWiimoteDiscovered;
            Wii.OnWiimoteDisconnected += OnWiimoteDisconnected;
        }

        public void ShutDown()
        {
            Wii.OnDiscoveryFailed     -= OnDiscoveryFailed;
            Wii.OnWiimoteDiscovered   -= OnWiimoteDiscovered;
            Wii.OnWiimoteDisconnected -= OnWiimoteDisconnected;
        }

        void OnDiscoveryFailed(int i)
        {
            Debug.Log("Error:" + i + ". Try Again.");
            //searching = false;
        }
        
        public void BeginSearch()
        {
            for(var x=0;x<16;x++)
            {
                if(Wii.IsActive(x))
                {
                    CheckIfConnectionDeviceIsRemoteOrBalanceBoard(x);
                }
            }
        
        
            //searching = true;
            Wii.StartSearch();   
            Time.timeScale = 1.0f;
            TotalDevices = Wii.GetRemoteCount();
        }

        public void CheckForMotionPlus()
        {
            if (motionPlusDetected || wiiMotionPlusDeviceNr < 0)
                return;
            
            Debug.LogWarning("CHECKFORMOTIONPLUS");
            Wii.CheckForMotionPlus(wiiMotionPlusDeviceNr);

        }

        public void CalibrateMotionPlus()
        {
            if (wiiMotionPlusDeviceNr < 0)
                return;
            
            Debug.LogWarning("CALIBRATEMOTIONPLUS");
            
            Debug.Log("Calibrate WiiMotionPlus");
            Wii.CalibrateMotionPlus(wiiMotionPlusDeviceNr);

        }

        public bool IsWiiMotionCalibrated()
        {
            return Wii.IsMotionPlusCalibrated(wiiMotionPlusDeviceNr);
        }

        public bool IsWiiBalanceBoardActive()
        {
            return Wii.GetExpType(balanceBoardDeviceNr) == 3;
        }
        
        void OnWiimoteDiscovered (int thisRemote) 
        {
            Debug.Log("found this one: "+thisRemote);
            if (!Wii.IsActive(currentDevice))
                currentDevice = thisRemote;
                
            CheckIfConnectionDeviceIsRemoteOrBalanceBoard(thisRemote);

            TotalDevices = Wii.GetRemoteCount();
        }
	
        void OnWiimoteDisconnected (int thisRemote) 
        {
            Debug.Log("lost this one: "+ thisRemote);

            IsDisconnectedDeviceRemoteOrBalanceBoard(thisRemote);
        
            TotalDevices = Wii.GetRemoteCount();
        }

        
        
        bool CheckIfConnectionDeviceIsRemoteOrBalanceBoard(int thisRemote)
        {
            switch (Wii.GetExpType(thisRemote))
            {
                case 0: //Normal Wii Mote
                    WiiMotionPlusDeviceNr = thisRemote;
                    
                    if (Wii.HasMotionPlus(thisRemote)) //Motion Plus?
                    {
                        motionPlusDetected = true;
                        
                        WiiMotionPlusDeviceNr = thisRemote;
                        isWiiMotionPlusActive = true;

                        Debug.LogWarning(currentDevice + ": Wii Motion Plus Controller.");
                        CoreEventManager.Instance.GameEvents.OnIsWiiMotionPlusActive?.Invoke(true);
                        
                
                        return true;
                    }
            
                    Debug.LogWarning(currentDevice + ": NO Motion Plus!!!");

                    return false; //It doesnt have Motion Plus and therefore can not be used;
            
                case 3: //Balance Board

                    balanceBoardDetected = true;
                    
                    BalanceBoardDeviceNr = thisRemote;
                    isBalanceBoardActive = true;
                    
                    Debug.LogWarning("BalanceBoard detected");
                    CoreEventManager.Instance.GameEvents.OnIsWiiBalanceBoardActive?.Invoke(true);
                    

                    return true;
            }

            return false;
        }

        bool IsDisconnectedDeviceRemoteOrBalanceBoard(int thisRemote)
        {
            if (thisRemote == WiiMotionPlusDeviceNr)
            {
                Debug.LogError(thisRemote + ": disconnected Wii Motion Plus Controller!");
                WiiMotionPlusDeviceNr = -1;

                return true;
            }

            if (thisRemote == BalanceBoardDeviceNr)
            {
                Debug.LogError(thisRemote + ": disconnected Balance Board!");
                BalanceBoardDeviceNr = -1;
            
                return true;
            }
        
            return false;
        }
        
    }
}
