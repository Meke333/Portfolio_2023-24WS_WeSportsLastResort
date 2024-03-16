using System;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieShieldGraphic : MonoBehaviour
    {
        private ZombieScript zombieScript;

        [Header("ShieldGraphics")] 
        public ShieldGraphicHolder[] ShieldGraphicHolders;

        #region UnityMethods

        private void Awake()
        {
            zombieScript = transform.GetComponentInParent<ZombieScript>();
        }

        private void OnEnable()
        {
            //zombieScript.onSettingShield += ProcessAction_onSettingShield;
            zombieScript.onSettingNewZombieGuardPosition += ProcessAction_onSettingNewZombieGuardPosition;
        }

        private void OnDisable()
        {
            //zombieScript.onSettingShield -= ProcessAction_onSettingShield;
            zombieScript.onSettingNewZombieGuardPosition -= ProcessAction_onSettingNewZombieGuardPosition;
        }

        #endregion

        #region Methods

        void SetGraphicToWeaponType()
        {
            //needs further implementing if there are more graphics to set up
            ShieldGraphicHolders[0].gameObject.SetActive(true);
        }

        #endregion


        #region Event Methods

        void ProcessAction_onSettingNewZombieGuardPosition(ZombieGuardDirection direction)
        {
            if (direction == ZombieGuardDirection.None)
            {
                for (int i = 0; i < ShieldGraphicHolders.Length; i++)
                {
                    ShieldGraphicHolders[i].gameObject.SetActive(false);
                }
                return;
            }

            SetGraphicToWeaponType();
        }
        
        #endregion
        
    }

    [System.Serializable]
    public struct ShieldGraphicHolder
    {
        public GameObject gameObject;
        public ShieldType shieldType;
    }

    public enum ShieldType
    {
        None,
        TrashBinCover,
        
    }
}
