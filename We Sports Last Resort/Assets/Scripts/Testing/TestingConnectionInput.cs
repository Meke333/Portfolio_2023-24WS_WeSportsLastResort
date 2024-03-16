using System;
using TMPro;
using UnityEngine;
using WiiScripts.Input;

namespace Testing
{
    public class TestingConnectionInput : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI motionControlsData;
        [SerializeField] private TextMeshProUGUI buttonData;
        [SerializeField] private TextMeshProUGUI balanceBoardData;
        [SerializeField] private TextMeshProUGUI iRData;
        [SerializeField] private TextMeshProUGUI accelerationData;

        private bool isMotionControlActive;
        private bool isBalanceBoardActive;

        private float weight;
        private Vector2 cog;
        private Vector4 wD;

        
        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            WiiMoteInput.Instance.OnWiiRemoteActiveChange += ProcessActionOnWiiRemoteActiveChange;
            WiiMoteInput.Instance.OnButton_A += ProcessInput_A;
            WiiMoteInput.Instance.OnMotion_GetMotionPlus += ProcessMotionControls;
            WiiMoteInput.Instance.OnIR_GetIRPosition += ProcessIRPosition;
            WiiMoteInput.Instance.OnAcceleration_GetWiiAcceleration += ProcessAcceleration;
            
            
            WiiBalanceBoardInput.Instance.OnBalanceBoardActiveChange += ProcessActionOnBalanceBoardActiveChange;
            WiiBalanceBoardInput.Instance.OnTotalWeightChange += ProcessTotalWeight;
            WiiBalanceBoardInput.Instance.OnWeightDistributionChange += ProcessWeightDistribution;
            WiiBalanceBoardInput.Instance.OnCenterOfBalanceChange += ProcessCOG;

        }


        void ProcessActionOnWiiRemoteActiveChange(bool value)
        {
            isMotionControlActive = value;
        }

        void ProcessActionOnBalanceBoardActiveChange(bool value)
        {
            isBalanceBoardActive = value;
        }

        void ProcessInput_A(bool[] a)
        {
            if (isMotionControlActive)
                buttonData.text = "button A: GetButton: " + a[0] + "\n GetButtonDown: " + a[1] + "\n GetButtonUp" + a[2];
        }

        void ProcessMotionControls(Vector3 m)
        {
            if (isMotionControlActive)
            {
                motionControlsData.text = "Motion Control: \n Yaw: " + m.x + "; \n Pitch: " + m.y + "; \n Roll: " + m.z;
            }
        }

        void ProcessIRPosition(Vector2 pos)
        {
            if (isMotionControlActive)
                iRData.text = "IR: \n x: " + pos.x + "\n y: " + pos.y;
        }

        void ProcessAcceleration(Vector3 h)
        {
            if (isMotionControlActive)
                accelerationData.text = "Acceleration: " + h;
        }

        void ProcessWeightDistribution(Vector4 a)
        {
            if (isBalanceBoardActive)
            {
                wD = a;
                DisplayBalanceBoardText();
            }

        }

        void ProcessCOG(Vector2 b)
        {
            if (isBalanceBoardActive)
            {
                cog = b;
                DisplayBalanceBoardText();
            }
        }

        void ProcessTotalWeight(float c)
        {
            if (isBalanceBoardActive)
            {
                weight = c;
                DisplayBalanceBoardText();
            }
        }

        void DisplayBalanceBoardText()
        {
            balanceBoardData.text = "Balance Board: \n total Weight: " + weight + "\n distribution: \n x: " + wD.x +
                                    "\n y: " + wD.y + "\n z: " + wD.z + "\n w: " + wD.w + "\n cog: \n x:" + cog.x +
                                    "\n y: " + cog.y;
        }
        
        
    }
}
