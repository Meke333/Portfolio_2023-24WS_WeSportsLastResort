using System;
using PlayerScripts.StateMachines.Weapon;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    [System.Serializable]
    public class Idle : PlayerStateMachine
    {
        

        public Idle(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Idle";
            playerState = PlayerStateEnum.Idle;
        }

        public override void Enter()
        {
            base.Enter();
            
            
        }

        public override void Exit()
        {
            base.Exit();
        }

        #region EventMethods

        protected override void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast)
        {
            playerStateVariableContainer.IsWiiMotePitchOrYawFast = isFast;

            if (!isFast)
                return;
            
            //If the WiiMote is swinging fast then switch to Attacking State
            nextState = new Attacking(playerStateVariableContainer);
            stage = EVENT.EXIT;

        }

        protected override void ProcessAction_onWiiMote_GetButtons(bool[][] buttons)
        {
            base.ProcessAction_onWiiMote_GetButtons(buttons);

            switch (playerStateVariableContainer.WeaponType)
            {
                case WeaponType.None:
                    Debug.Log("Idle GetButtons: There is no Weapon...");
                    return;
                case WeaponType.Sword:
                    if (playerStateVariableContainer.Buttons[1][1]) //[B][GetButtonDown]
                    {
                        //Blocking with Sword
                        nextState = new Defending(playerStateVariableContainer);
                        stage = EVENT.EXIT;
                    }
                    break;
                case WeaponType.Gun:
                    if (playerStateVariableContainer.Buttons[0][0]) //[A][GetButton]
                    {   
                        //Shooting the Gun
                        nextState = new Attacking(playerStateVariableContainer);
                        stage = EVENT.EXIT;
                    }
                    break;
            }

            
        }

        #endregion
    }
}
