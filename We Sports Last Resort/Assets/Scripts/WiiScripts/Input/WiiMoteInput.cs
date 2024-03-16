using System;
using Core;
using General.Helper;
using General.SingletonClass;
using UnityEngine;
using UnityEngine.Serialization;

namespace WiiScripts.Input
{
    public class WiiMoteInput : SingletonClass<WiiMoteInput>
    {
        #region Parameters

        public int motionPlusDeviceNr;
        public bool isMotionPlusRemoteActive;

        //Buttons
        //bool[3]: {GetButton, GetButtonDown, GetButtonUp}
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

        #region Properties

        public bool[] ButtonA
        {
            get => _buttonA;
            set
            {
                
                if (OperationHelper.CompareSameLengthArrays(_buttonA, value))
                    return;
                
                _buttonA = value;
                OnButton_A?.Invoke(_buttonA);
            }
        }
        public bool[] ButtonB
        {
            get => _buttonB;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonB, value))
                    return;
                
                _buttonB = value;
                OnButton_B?.Invoke(_buttonB);
            }
        }
        public bool[] ButtonOne
        {
            get => _button1;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_button1, value))
                    return;
                
                _button1 = value;
                OnButton_ONE?.Invoke(_button1);
            }
        }
        public bool[] ButtonTwo
        {
            get => _button2;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_button2, value))
                    return;
                
                _button2 = value;
                OnButton_TWO?.Invoke(_button2);
            }
        }
        public bool[] ButtonDUp
        {
            get => _buttonDUp;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonDUp, value))
                    return;
                
                _buttonDUp = value;
                OnButton_D_UP?.Invoke(_buttonDUp);
            }
        }
        public bool[] ButtonDDown
        {
            get => _buttonDDown;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonDDown, value))
                    return;
                
                _buttonDDown = value;
                OnButton_D_DOWN?.Invoke(_buttonDDown);
            }
        }
        public bool[] ButtonDLeft
        {
            get => _buttonDLeft;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonDLeft, value))
                    return;
                
                _buttonDLeft = value;
                OnButton_D_LEFT?.Invoke(_buttonDLeft);
            }
        }
        public bool[] ButtonDRight
        {
            get => _buttonDRight;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonDRight, value))
                    return;
                
                _buttonDRight = value;
                OnButton_D_RIGHT?.Invoke(_buttonDRight);
            }
        }
        public bool[] ButtonPlus
        {
            get => _buttonPlus;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonPlus, value))
                    return;
                
                _buttonPlus = value;
                OnButton_PLUS?.Invoke(_buttonPlus);
            }
        }
        public bool[] ButtonMinus
        {
            get => _buttonMinus;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonMinus, value))
                    return;
                
                _buttonMinus = value;
                OnButton_MINUS?.Invoke(_buttonMinus);
            }
        }
        public bool[] ButtonHome
        {
            get => _buttonHome;
            set
            {
                if (OperationHelper.CompareSameLengthArrays(_buttonHome, value))
                    return;
                
                _buttonHome = value;
                OnButton_HOME?.Invoke(_buttonHome);
            }
        }

        #endregion
        
        private readonly int _totalButtons = 11;
        private readonly int _totalButtonData = 3;

        //Acceleration
        private Vector3 _acceleration;
        
        //Motion
        private Vector3 _motionPlus;
        private bool[] _fastMovement = new bool[3]; //Yaw, Pitch, Roll

        //IR
        private Vector2 _pointerPosition;
        private float _pointerRotation;
        
        //Lights
        //bool[4]: {LED1, LED2, LED3, LED4}
        private bool[] _led = new bool[4];

        #endregion

        #region Events

        public event Action<bool> OnWiiRemoteActiveChange;
        public event Action<bool> OnMotionPlusActiveChange;
        
        //Buttons
        //bool[3]: {GetButton, GetButtonDown, GetButtonUp}
        public event Action<bool[]> OnButton_A;
        public event Action<bool[]> OnButton_B;
        public event Action<bool[]> OnButton_ONE;
        public event Action<bool[]> OnButton_TWO;
        public event Action<bool[]> OnButton_D_UP;
        public event Action<bool[]> OnButton_D_DOWN;
        public event Action<bool[]> OnButton_D_LEFT;
        public event Action<bool[]> OnButton_D_RIGHT;
        public event Action<bool[]> OnButton_PLUS;
        public event Action<bool[]> OnButton_MINUS;
        public event Action<bool[]> OnButton_HOME;
        
        //Acceleration
        public event Action<Vector3> OnAcceleration_GetWiiAcceleration;
        
        //Motion
        public event Action<Vector3> OnMotion_GetMotionPlus;
        //public event Action<bool[]> OnMotion_IsYawPitchRollFast; //bool[3]: {Yaw, Pitch, Roll}
        public event Action<bool> OnMotion_IsYawFast;
        public event Action<bool> OnMotion_IsPitchFast;
        public event Action<bool> OnMotion_IsRollFast;

        //IR
        public event Action<Vector2> OnIR_GetIRPosition;
        public event Action<float> OnIR_GetIRRotation;

        //Lights
        //bool[4]: {LED1, LED2, LED3, LED4}
        public event Action<bool[]> OnLightsChange;

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            #region Observer

            CoreWiiManager.Instance.onWiiMotionPlusDeviceChange += EvaluateAction_onWiiMotionPlusDeviceChange;

            #endregion
        }

        private void OnDisable()
        {
            #region Observer

            CoreWiiManager.Instance.onWiiMotionPlusDeviceChange -= EvaluateAction_onWiiMotionPlusDeviceChange;

            #endregion
        }

        private void Update()
        {
            ProcessButtonInputs();
            ProcessAcceleration();
            ProcessMotionPlus();
            ProcessIRInput();
        }

        #endregion

        #region Methods

        private void ProcessButtonInputs()
        {
            ButtonA = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.A),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.A),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.A)
            };
            ButtonB = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.B),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.B),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.B)
            };
            ButtonOne = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.ONE),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.ONE),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.ONE)
            };
            ButtonTwo = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.TWO),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.TWO),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.TWO)
            };
            ButtonDUp = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.D_UP),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.D_UP),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.D_UP)
            };
            ButtonDDown = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.D_DOWN),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.D_DOWN),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.D_DOWN)
            };
            ButtonDLeft = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.D_LEFT),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.D_LEFT),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.D_LEFT)
            };
            ButtonDRight = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.D_RIGHT),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.D_RIGHT),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.D_RIGHT)
            };
            ButtonPlus = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.PLUS),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.PLUS),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.PLUS)
            };
            ButtonMinus = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.MINUS),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.MINUS),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.MINUS)
            };
            ButtonHome = new bool[]
            {
                Wii.GetButton(motionPlusDeviceNr, WiiButtonsEnum.HOME),
                Wii.GetButtonDown(motionPlusDeviceNr, WiiButtonsEnum.HOME),
                Wii.GetButtonUp(motionPlusDeviceNr, WiiButtonsEnum.HOME)
            };

        }

        private void ProcessAcceleration()
        {
            Vector3 temp = Wii.GetWiimoteAcceleration(motionPlusDeviceNr);
            if (_acceleration == temp)
                return;
            
            _acceleration = temp;
            OnAcceleration_GetWiiAcceleration?.Invoke(_acceleration);
        }
        
        private void ProcessMotionPlus()
        {
            Vector3 temp = Wii.GetMotionPlus(motionPlusDeviceNr);
            if (_motionPlus == temp)
                return;
            
            _motionPlus = temp;
            OnMotion_GetMotionPlus?.Invoke(_motionPlus);

            bool isYawFast = Wii.IsYawFast(motionPlusDeviceNr);
            bool isPitchFast = Wii.IsPitchFast(motionPlusDeviceNr);
            bool isRollFast = Wii.IsRollFast(motionPlusDeviceNr);
            if (_fastMovement[0] != isYawFast)
            {
                _fastMovement[0] = isYawFast;
                OnMotion_IsYawFast?.Invoke(isYawFast);
            } 
            if (_fastMovement[1] != isPitchFast)
            {
                _fastMovement[1] = isPitchFast;
                OnMotion_IsPitchFast?.Invoke(isYawFast);
            } 
            if (_fastMovement[2] != isRollFast)
            {
                _fastMovement[2] = isRollFast;
                OnMotion_IsRollFast?.Invoke(isRollFast);
            } 
        }

        private void ProcessIRInput()
        {
            Vector2 tempPos = Wii.GetIRPosition(motionPlusDeviceNr);
            float tempRot = Wii.GetIRRotation(motionPlusDeviceNr);
            if (_pointerPosition != tempPos)
            {
                _pointerPosition = tempPos;
                OnIR_GetIRPosition?.Invoke(_pointerPosition);
            }
            if (Math.Abs(_pointerRotation - tempRot) > 0.01f)
            {
                _pointerRotation = tempRot;
                OnIR_GetIRRotation?.Invoke(_pointerRotation);
            }
        }

        #endregion

        #region EventMethods

        void EvaluateAction_onWiiMotionPlusDeviceChange(int deviceNr)
        {
            motionPlusDeviceNr = deviceNr;
            isMotionPlusRemoteActive = motionPlusDeviceNr >= 0;
            
            OnWiiRemoteActiveChange?.Invoke(isMotionPlusRemoteActive);
        }

        #endregion
    }
}
