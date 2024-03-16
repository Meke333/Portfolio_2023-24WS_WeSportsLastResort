using System;
using Core;
using UnityEngine;

namespace UI
{
    public class UIWinScreen : MonoBehaviour
    {
        [SerializeField] private GameObject winUIGameObject;

        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnLevel1Finished += ProcessAction_OnLevel1Finished;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnLevel1Finished -= ProcessAction_OnLevel1Finished;
        }

        void ProcessAction_OnLevel1Finished()
        {
            winUIGameObject.SetActive(true);
        }
    }
}
