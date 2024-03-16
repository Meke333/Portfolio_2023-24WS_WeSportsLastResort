using System;
using PlayerScripts.StateMachines.Weapon;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    public class Defending : PlayerStateMachine
    {
        public Defending(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Defending";
            playerState = PlayerStateEnum.Blocking;
        }

        public override void Enter()
        {
            base.Enter();
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_isDefending?.Invoke(true);

        }

        public override void Exit()
        {
            base.Exit();
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_isDefending?.Invoke(false);

        }


        protected override void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast)
        {
            return;
        }
        
        protected override void ProcessAction_onWiiMote_GetButtons(bool[][] buttons)
        {
            base.ProcessAction_onWiiMote_GetButtons(buttons);

            switch (playerStateVariableContainer.WeaponType)
            {
                case WeaponType.None:
                    Debug.Log("Defending GetButtons: There is no Weapon...");
                    return;
                case WeaponType.Sword:
                    if (playerStateVariableContainer.Buttons[1][2]) //[B][GetButtonUp]
                    {
                        //NotBlocking anymore
                        nextState = new Idle(playerStateVariableContainer);
                        stage = EVENT.EXIT;
                    }
                    break;
                case WeaponType.Gun:
                    Debug.Log("Defending GetButtons: HOW DID YOU GET HERE WITH THE GUN?!?!?! FIX!");
                    break;
            }
        }
    }
}
