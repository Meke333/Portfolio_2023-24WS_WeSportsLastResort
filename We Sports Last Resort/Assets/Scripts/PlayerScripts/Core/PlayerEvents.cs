using System;
using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using PlayerScripts.StateMachines.Player;
using PlayerScripts.StateMachines.Weapon;
using UnityEngine;

namespace PlayerScripts.Core
{
    public class PlayerEvents
    {
        
        #region WiiMote

        public Action<bool> onWiiMote_IsWiiMoteActive;
        public Action<bool> onWiiMote_IsMotionPlusActive;
        
        //Buttons
        //bool[3]: {GetButton, GetButtonDown, GetButtonUp}
        /*public Action<bool[]> onWiiMote_Button_A;
        public Action<bool[]> onWiiMote_Button_B;
        public Action<bool[]> onWiiMote_Button_ONE;
        public Action<bool[]> onWiiMote_Button_TWO;
        public Action<bool[]> onWiiMote_Button_D_UP;
        public Action<bool[]> onWiiMote_Button_D_DOWN;
        public Action<bool[]> onWiiMote_Button_D_LEFT;
        public Action<bool[]> onWiiMote_Button_D_RIGHT;
        public Action<bool[]> onWiiMote_Button_PLUS;
        public Action<bool[]> onWiiMote_Button_MINUS;
        public Action<bool[]> onWiiMote_Button_HOME;
        */
        //bool[11][]: [ButtonType][GetButton/GetButtonDown/GetButtonUp Information]
        public Action<bool[][]> onWiiMote_GetButtons;
        //Acceleration
        public Action<Vector3> onWiiMote_GetAcceleration;
        
        //Motion Plus
        public Action<Vector3> onWiiMote_GetMotionPlus;
        public Action<bool> onWiiMote_IsYawFast;
        public Action<bool> onWiiMote_IsPitchFast;
        public Action<bool> onWiiMote_IsRollFast;
        
        //Lights
        //bool[4]: {LED1, LED2, LED3, LED4}
        public Action<bool[]> OnWiiMote_LEDChange;
        
        #endregion

        #region BalanceBoard

        public Action<Vector2> onBalanceBoard_CenterOfGravity; 

        #endregion
        
        #region Movement

        public Action<int> onMovement_TakingAStep; //int: direction (-1 left, 1 right)
        public Action onMovement_StretchingFromBentPosition;
        public Action<float> onMovement_MovingToSides;             //float: speed (<0 left; >0 right)
        public Action<float> onMovement_ChangeSpeed;

        #endregion

        #region PlayerStatemachine

        public Action<PlayerStateEnum> onPlayerStateMachine_onStateChange;
        
        public Action<bool> onPlayerStateMachine_isAttacking;
        public Action<bool> onPlayerStateMachine_isDefending;

        public Action onPlayerStateMachine_TriggerStunState;
        public Action<float> onPlayerStateMachine_TakingDamage;

        #endregion

        #region WeaponStateMachine

        public Action<WeaponType> onWeaponStateMachine_TransmitNewWeaponType;

        #endregion

        #region Weapons

        public Action<bool> onWeapons_ActivateHitbox;

        #endregion

        #region Enemies

        public Action<ZombieScript> onEnemies_FocusingZombie;
        public Action onEnemies_ZombieDied;

        #endregion
    }
}
