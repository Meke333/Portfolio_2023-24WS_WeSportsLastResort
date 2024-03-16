using PlayerScripts.Core;
using UnityEngine;
using WiiScripts.Input;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerProcessWiiMote
    {
        #region Parameter

        private PlayerScript _playerScript;
        
        //Active
        private bool _isWiiMoteActive;

        #region Buttons

        private bool[] _buttonA = new bool[3];
        private bool[] _buttonB = new bool[3];
        private bool[] _button1 = new bool[3];
        private bool[] _button2 = new bool[3];
        private bool[] _buttonDUp = new bool[3];
        private bool[] _buttonDDown = new bool[3];
        private bool[] _buttonDLeft = new bool[3];
        private bool[] _buttonDRight = new bool[3];
        private bool[] _buttonPlus = new bool[3];
        private bool[] _buttonMinus = new bool[3];
        private bool[] _buttonHome = new bool[3];

        #endregion
        
        //Acceleration
        private Vector3 _acceleration;
        
        //Motion Plus
        private Vector3 _motionPlus;
        private Vector3 _wmpOffset;

        private bool _isYawFast;
        private bool _isPitchFast;
        private bool _isRollFast;
        
        //Lights
        private bool[] _ledLights = new bool[4];


        #endregion

        //Constructor
        public PlayerProcessWiiMote(PlayerScript newPlayerScript)
        {
            _playerScript = newPlayerScript;
        }
        
        #region Start & End Methods



        public void Initialize()
        {
            WiiMoteInput.Instance.OnWiiRemoteActiveChange += ProcessAction_OnWiiRemoteActiveChange;
            
            //Buttons
            WiiMoteInput.Instance.OnButton_A += ProcessAction_OnButton_A;
            WiiMoteInput.Instance.OnButton_B += ProcessAction_OnButton_B;
            WiiMoteInput.Instance.OnButton_ONE += ProcessAction_OnButton_ONE;
            WiiMoteInput.Instance.OnButton_TWO += ProcessAction_OnButton_TWO;
            WiiMoteInput.Instance.OnButton_D_UP += ProcessAction_OnButton_D_UP;
            WiiMoteInput.Instance.OnButton_D_DOWN += ProcessAction_OnButton_D_DOWN;
            WiiMoteInput.Instance.OnButton_D_LEFT += ProcessAction_OnButton_D_LEFT;
            WiiMoteInput.Instance.OnButton_D_RIGHT += ProcessAction_OnButton_D_RIGHT;
            WiiMoteInput.Instance.OnButton_PLUS += ProcessAction_OnButton_PLUS;
            WiiMoteInput.Instance.OnButton_MINUS += ProcessAction_OnButton_MINUS;
            WiiMoteInput.Instance.OnButton_HOME += ProcessAction_OnButton_HOME;
            
            //Acceleration
            WiiMoteInput.Instance.OnAcceleration_GetWiiAcceleration += ProcessAction_OnAcceleration_GetWiiAcceleration;
            
            //Motion Plus
            WiiMoteInput.Instance.OnMotion_GetMotionPlus += ProcessAction_OnMotion_GetMotionPlus;
            WiiMoteInput.Instance.OnMotion_IsYawFast += ProcessAction_OnMotion_IsYawFast;
            WiiMoteInput.Instance.OnMotion_IsPitchFast += ProcessAction_OnMotion_IsPitchFast;
            WiiMoteInput.Instance.OnMotion_IsRollFast += ProcessAction_OnMotion_IsRollFast;

            //Lights
            _playerScript.PlayerEvents.OnWiiMote_LEDChange += ProcessAction_OnWiiMote_LEDChange;

        }

        public void ShutDown()
        {
            WiiMoteInput.Instance.OnWiiRemoteActiveChange -= ProcessAction_OnWiiRemoteActiveChange;
            
            //Buttons
            WiiMoteInput.Instance.OnButton_A -= ProcessAction_OnButton_A;
            WiiMoteInput.Instance.OnButton_B -= ProcessAction_OnButton_B;
            WiiMoteInput.Instance.OnButton_ONE -= ProcessAction_OnButton_ONE;
            WiiMoteInput.Instance.OnButton_TWO -= ProcessAction_OnButton_TWO;
            WiiMoteInput.Instance.OnButton_D_UP -= ProcessAction_OnButton_D_UP;
            WiiMoteInput.Instance.OnButton_D_DOWN -= ProcessAction_OnButton_D_DOWN;
            WiiMoteInput.Instance.OnButton_D_LEFT -= ProcessAction_OnButton_D_LEFT;
            WiiMoteInput.Instance.OnButton_D_RIGHT -= ProcessAction_OnButton_D_RIGHT;
            WiiMoteInput.Instance.OnButton_PLUS -= ProcessAction_OnButton_PLUS;
            WiiMoteInput.Instance.OnButton_MINUS -= ProcessAction_OnButton_MINUS;
            WiiMoteInput.Instance.OnButton_HOME -= ProcessAction_OnButton_HOME;
            
            //Acceleration
            WiiMoteInput.Instance.OnAcceleration_GetWiiAcceleration -= ProcessAction_OnAcceleration_GetWiiAcceleration;
            
            //Motion Plus
            WiiMoteInput.Instance.OnMotion_GetMotionPlus -= ProcessAction_OnMotion_GetMotionPlus;
            WiiMoteInput.Instance.OnMotion_IsYawFast -= ProcessAction_OnMotion_IsYawFast;
            WiiMoteInput.Instance.OnMotion_IsPitchFast -= ProcessAction_OnMotion_IsPitchFast;
            WiiMoteInput.Instance.OnMotion_IsRollFast -= ProcessAction_OnMotion_IsRollFast;

            //Lights
            _playerScript.PlayerEvents.OnWiiMote_LEDChange -= ProcessAction_OnWiiMote_LEDChange;

            
        }
        

        #endregion

        #region Methods

        private void MessageButtons()
        {
            //bool[a][b]: a = Button-Type, b = GetButton/GetButtonDown/GetButtonUp Information
            bool[][] message = new bool[11][];
            message[0] = _buttonA;
            message[1] = _buttonB;
            message[2] = _button1;
            message[3] = _button2;
            message[4] = _buttonDUp;
            message[5] = _buttonDDown;
            message[6] = _buttonDLeft;
            message[7] = _buttonDRight;
            message[8] = _buttonPlus;
            message[9] = _buttonMinus;
            message[10] = _buttonHome;
            
            _playerScript.PlayerEvents.onWiiMote_GetButtons?.Invoke(message);
        }
        
        private void ResetWMPOffset()
        {
            
            
            
        }

        private void RecalibrateWiiMotionPlus()
        {
            
            
        }

        #endregion

        #region EventMethods

        void ProcessAction_OnWiiRemoteActiveChange(bool isActive)
        {
            _isWiiMoteActive = isActive;
                
            _playerScript.PlayerEvents.onWiiMote_IsWiiMoteActive?.Invoke(_isWiiMoteActive);
        }

        void ProcessAction_OnMotionPlusActiveChange(bool isActive)
        {
            
        }
        
        //Buttons
        void ProcessAction_OnButton_A(bool[] button)
        {
            _buttonA = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_B(bool[] button)
        {
            _buttonB = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_ONE(bool[] button)
        {
            _button1 = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_TWO(bool[] button)
        {
            _button2 = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_D_UP(bool[] button)
        {
            _buttonDUp = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_D_DOWN(bool[] button)
        {
            _buttonDDown = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_D_LEFT(bool[] button)
        {
            _buttonDLeft = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_D_RIGHT(bool[] button)
        {
            _buttonDRight = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_PLUS(bool[] button)
        {
            _buttonPlus = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_MINUS(bool[] button)
        {
            _buttonMinus = button;
            MessageButtons();
        }
        void ProcessAction_OnButton_HOME(bool[] button)
        {
            _buttonHome = button;
            MessageButtons();
        }
        
        //Acceleration
        void ProcessAction_OnAcceleration_GetWiiAcceleration(Vector3 acceleration)
        {
            _acceleration = acceleration;
            _playerScript.PlayerEvents.onWiiMote_GetAcceleration?.Invoke(_acceleration);
        }
        
        //Motion Plus
        void ProcessAction_OnMotion_GetMotionPlus(Vector3 mp)
        {
            _motionPlus = mp;
            _playerScript.PlayerEvents.onWiiMote_GetMotionPlus?.Invoke(_motionPlus /*+ _wmpOffset*/);
        }
        void ProcessAction_OnMotion_IsYawFast(bool y)
        {
            _isYawFast = y;
            _playerScript.PlayerEvents.onWiiMote_IsYawFast?.Invoke(_isYawFast);
        }
        void ProcessAction_OnMotion_IsPitchFast(bool p)
        {
            _isPitchFast = p;
            _playerScript.PlayerEvents.onWiiMote_IsPitchFast?.Invoke(_isPitchFast);
        }
        void ProcessAction_OnMotion_IsRollFast(bool r)
        {
            _isRollFast = r;
            _playerScript.PlayerEvents.onWiiMote_IsRollFast?.Invoke(_isRollFast);
        }
        
        //Lights
        void ProcessAction_OnWiiMote_LEDChange(bool[] led)
        {
            _ledLights = led;
            Wii.SetLEDs(WiiMoteInput.Instance.motionPlusDeviceNr, 
                _ledLights[0], 
                _ledLights[1], 
                _ledLights[2],
                _ledLights[3]);
        }
        
        
        #endregion
    }
}
