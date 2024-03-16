using PlayerScripts.Core;
using PlayerScripts.StateMachines.Weapon;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player
{
    public class PlayerStateVariableContainer
    {
        public PlayerScript PlayerScript;
        public PlayerEvents PlayerEvents;
        
        //Weapon
        public WeaponType WeaponType;
        public float WeaponFullAttackTime;
        
        //Input
        public bool[][] Buttons;

        #region Buttons Indexes

        /*
        [][] = [ButtonType][GetButton/GetButtonDown/GetButtonUp];
        
        [0][...] = A
        [1][...] = B
        [2][...] = 1
        [3][...] = 2
        [4][...] = D-Pad Up
        [5][...] = D-Pad Down
        [6][...] = D-Pad Left
        [7][...] = D-Pad Right
        [8][...] = Plus
        [9][...] = Minus
        [10][...] = Home
        
        */

        #endregion
        
        public bool IsWiiMotePitchOrYawFast;

        //Constructor
        public PlayerStateVariableContainer(PlayerScript playerScript)
        {
            PlayerScript = playerScript;
            PlayerEvents = playerScript.PlayerEvents;
        }

        #region Initializing & ShutDown
        
        public void Initializing()
        {
            PlayerEvents.onWeaponStateMachine_TransmitNewWeaponType += ProcessAction_onWeaponStateMachine_TransmitNewWeaponType;
        }

        public void ShutDown()
        {
            PlayerEvents.onWeaponStateMachine_TransmitNewWeaponType -= ProcessAction_onWeaponStateMachine_TransmitNewWeaponType;
        }

        #endregion

        #region EventMethods


        void ProcessAction_onWeaponStateMachine_TransmitNewWeaponType(WeaponType wt)
        {
            WeaponType = wt;
            WeaponFullAttackTime = WeaponLUT.GetWeaponFullAttackTime(WeaponType);
            Debug.Log("new WeaponAttackTime = " + WeaponFullAttackTime);
        }


        #endregion
    }
}
