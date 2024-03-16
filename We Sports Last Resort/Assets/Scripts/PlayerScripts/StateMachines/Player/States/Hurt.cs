using General.Helper;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    public class Hurt : PlayerStateMachine
    {
        private float _hurtTime = 0.3f;
        private Timer _hurtTimer;
        public Hurt(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Hurt";
            playerState = PlayerStateEnum.Hurt;

            _hurtTimer = new Timer(_hurtTime);
            _hurtTimer.onTimerDone += ProcessAction_stunnedTimer_onTimerDone;
        }

        public override void Enter()
        {
            _hurtTimer.RunTimer();
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
            nextState = new Idle(playerStateVariableContainer);
            stage = EVENT.EXIT;
        }
        
        protected override void ProcessAction_onPlayerStateMachine_TriggerStunState()
        {
            base.ProcessAction_onPlayerStateMachine_TriggerStunState();
            
            _hurtTimer.InterruptTimer();
        }

        protected override void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            base.ProcessAction_onPlayerStateMachine_TakingDamage(damage);
            
            _hurtTimer.InterruptTimer();
        }

        #endregion
    }
}
