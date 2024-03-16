using General.Helper;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    [System.Serializable]
    public class Attacking : PlayerStateMachine
    {
        //Events invoked (PlayerEvents):
        //onPlayerStateMachine_isAttacking
        //
        
        
        private Timer _fullAttackTimer;

        public Attacking(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Attacking";
            playerState = PlayerStateEnum.Attacking;

            _fullAttackTimer = new Timer(playerStateVariableContainer.WeaponFullAttackTime);
            _fullAttackTimer.onTimerDone += ProcessAction_fullAttackTimer_onTimerDone;
            
            
            
        }

        public override void Enter()
        {
            base.Enter();
            
            _fullAttackTimer.RunTimer();
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_isAttacking?.Invoke(true);
        }

        public override void Exit()
        {
            base.Exit();
            
            _fullAttackTimer.onTimerDone -= ProcessAction_fullAttackTimer_onTimerDone;
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_isAttacking?.Invoke(false);
        }

        #region Methods

        private void ResetAttackState()
        {
            _fullAttackTimer.ResetAndRunTimer();
        }

        #endregion

        #region EventMethods

        void ProcessAction_fullAttackTimer_onTimerDone()
        {
            if (playerStateVariableContainer.IsWiiMotePitchOrYawFast)
            {
                //Stay in Attacking State
                ResetAttackState();
                //Debug.Log("Reset Attacking State");
            }
            else
            {
                //Go back to Idle State
                nextState = new Idle(playerStateVariableContainer);
                stage = EVENT.EXIT;
            }
        }
        
        protected override void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast)
        {
            playerStateVariableContainer.IsWiiMotePitchOrYawFast = isFast;
        }

        protected override void ProcessAction_onPlayerStateMachine_TriggerStunState()
        {
            base.ProcessAction_onPlayerStateMachine_TriggerStunState();
            
            _fullAttackTimer.InterruptTimer();
        }

        protected override void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            base.ProcessAction_onPlayerStateMachine_TakingDamage(damage);
            
            _fullAttackTimer.InterruptTimer();
        }

        #endregion
    }
}
