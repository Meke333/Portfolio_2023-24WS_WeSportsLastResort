using System;
using General.Helper;
using PlayerScripts.StateMachines.Weapon;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    public class Stunned : PlayerStateMachine
    {
        private float _stunTime = 2.25f;
        private Timer _stunnedTimer;

        private bool _isBlocking;
        
        
        public Stunned(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Stunned";
            playerState = PlayerStateEnum.Stunned;

            _stunnedTimer = new Timer(_stunTime);
            _stunnedTimer.onTimerDone += ProcessAction_stunnedTimer_onTimerDone;
        }

        public override void Enter()
        {
            _stunnedTimer.RunTimer();
            base.Enter();
            
            
        }

        protected override void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast)
        {
            return;
        }

        #region EventMethods

        void ProcessAction_stunnedTimer_onTimerDone()
        {
            //Go back to Idle State
            
            Debug.Log("Stun finished");
            
            if (_isBlocking)
                nextState = new Defending(playerStateVariableContainer);
            else
                nextState = new Idle(playerStateVariableContainer);
            
            stage = EVENT.EXIT;
        }

        protected override void ProcessAction_onPlayerStateMachine_TriggerStunState()
        {
            base.ProcessAction_onPlayerStateMachine_TriggerStunState();
            
            _stunnedTimer.InterruptTimer();
        }

        protected override void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            base.ProcessAction_onPlayerStateMachine_TakingDamage(damage);
            
            _stunnedTimer.InterruptTimer();
        }

        protected override void ProcessAction_onWiiMote_GetButtons(bool[][] buttons)
        {
            base.ProcessAction_onWiiMote_GetButtons(buttons);

            switch (playerStateVariableContainer.WeaponType)
            {
                case WeaponType.None:
                    break;
                case WeaponType.Sword:

                    if (playerStateVariableContainer.Buttons[1][1])
                    {
                        _isBlocking = true;
                    }
                    else if (playerStateVariableContainer.Buttons[1][2])
                    {
                        _isBlocking = false;
                    }    
                    
                    playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_isDefending?.Invoke(_isBlocking);

                    break;
                case WeaponType.Gun:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
        
    }
}
