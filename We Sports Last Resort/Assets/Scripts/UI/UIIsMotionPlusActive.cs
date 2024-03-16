using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIIsMotionPlusActive : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnIsWiiMotionPlusActive += ProcessAction_OnIsWiiMotionPlusActive;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnIsWiiMotionPlusActive -= ProcessAction_OnIsWiiMotionPlusActive;

        }

        #region EventMethods

        void ProcessAction_OnIsWiiMotionPlusActive(bool isActive)
        {
            if (isActive)
                text.text = "Motion Plus is ready!";
            else
            {
                text.text = "NO MOTION PLUS FOUND: RESTART GAME!";
            }
        }

        #endregion
    }
}
