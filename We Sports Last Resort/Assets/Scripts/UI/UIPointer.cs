using System;
using System.Threading.Tasks;
using Interface;
using UnityEngine;
using WiiScripts.Input;

namespace UI
{
    public class UIPointer : MonoBehaviour
    {
        private Camera _mainCamera;
        private Vector2 _IRPosition;
        private Ray _pointerRay;
        private Rigidbody2D _rigidbody;

        private Vector2 _mousePosition;
        private bool _isMouseUsedForUI;
        
        private float screenWidth = (float) Screen.width;
        private float screenHeight = (float) Screen.height;

        private IUIButton _currentButtonSelected;

        //UI
        public RectTransform theIRMain;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private async void OnEnable()
        {
            await Task.Yield();
            WiiMoteInput.Instance.OnIR_GetIRPosition += ProcessAction_OnIR_GetIRPosition;
            WiiMoteInput.Instance.OnButton_A += ProcessAction_OnButton_A;
        }

        private void OnDisable()
        {
            WiiMoteInput.Instance.OnIR_GetIRPosition -= ProcessAction_OnIR_GetIRPosition;
            WiiMoteInput.Instance.OnButton_A -= ProcessAction_OnButton_A;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.U))
            {
                _isMouseUsedForUI = !_isMouseUsedForUI;
            }

            if (!_isMouseUsedForUI)
                return;
            
            

            _mousePosition = Input.mousePosition;
            theIRMain.position = _mousePosition;
            theIRMain.sizeDelta = new Vector2(50, 50);

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _currentButtonSelected?.PointerOutsideButton();
            
            //Debug.LogWarning("POINTER TRIGGER ENTER");
            
            _currentButtonSelected = other.gameObject.GetComponent<IUIButton>();
            _currentButtonSelected?.PointerInsideButton();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //Debug.LogWarning("POINTER TRIGGER EXIT");
            
            _currentButtonSelected?.PointerOutsideButton();
            _currentButtonSelected = null;
        }

        #region Events

        void SetIRUIPosition()
        {
            theIRMain.position = new Vector3(_IRPosition.x * screenWidth, _IRPosition.y * screenHeight);
            theIRMain.sizeDelta = new Vector2(50, 50);
            
        }

        void SetIRUIPositionByRigidBody()
        {
            _rigidbody.MovePosition(new Vector3(_IRPosition.x * screenWidth, _IRPosition.y * screenHeight));
        }

        #endregion

        #region EventMethods

        void ProcessAction_OnIR_GetIRPosition(Vector2 newPosition)
        {
            _IRPosition = newPosition;
            SetIRUIPosition();
            //SetIRUIPositionByRigidBody();

        }

        void ProcessAction_OnButton_A(bool[] type)
        {
            if (!type[1])
                return;
            
            //Debug.LogWarning("ButtonDown");

            _currentButtonSelected?.PointerSelect();
        }
        
        #endregion
    }
}
