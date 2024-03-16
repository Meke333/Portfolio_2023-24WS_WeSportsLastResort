using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerHealth
{
    public class UIPlayerHealth : MonoBehaviour
    {

        public int health;
        public Image[] hearts;
        public Image heartContainer;
        public Sprite Heart;

        public Animator deathScreenAnimator;

        [SerializeField] private GameObject pointerAndButtons;

        private readonly int deathHash = Animator.StringToHash("Death");


        #region UnityMethods

        private void OnEnable()
        {
            CoreEventManager.Instance.UIEvents.OnPlayerHealth += ProcessAction_UIEventts_OnPlayerHealth;
            CoreEventManager.Instance.GameEvents.OnPlayerDied += ProcessAction_GameEvents_OnPlayerDied;

        }

        private void OnDisable()
        {
            CoreEventManager.Instance.UIEvents.OnPlayerHealth -= ProcessAction_UIEventts_OnPlayerHealth;
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= ProcessAction_GameEvents_OnPlayerDied;

        }

        #endregion

        #region Methods

        void UpdateHealthUI()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].enabled = (i < health);
            }

            if (health <= 0)
            {
                heartContainer.enabled = false;
            }
        }

        #endregion

        #region EventMethods

        void ProcessAction_UIEventts_OnPlayerHealth(int hitpoints)
        {
            health = hitpoints;
            UpdateHealthUI();
        }

        void ProcessAction_GameEvents_OnPlayerDied()
        {
            deathScreenAnimator.SetTrigger(deathHash);
            pointerAndButtons.SetActive(true);
        }

        #endregion

        /*void Update()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
 
                }
            }
        }*/
    }
}
