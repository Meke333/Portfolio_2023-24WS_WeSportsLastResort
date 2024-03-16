using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIIsBalanceBoardActive : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnIsWiiBalanceBoardActive += ProcessAction_OnIsWiiBalanceBoardActive;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnIsWiiBalanceBoardActive -= ProcessAction_OnIsWiiBalanceBoardActive;

        }

        #region EventMethods

        void ProcessAction_OnIsWiiBalanceBoardActive(bool isActive)
        {
            if (isActive)
                text.text = "BalanceBoard is ready!";
            else
            {
                text.text = "NO BalanceBoard FOUND: RESTART GAME!";
            }
        }

        #endregion
    }
}
