using UnityEngine;

namespace PlayerScripts.StateMachines.Player.States
{
    public class Dead : PlayerStateMachine
    {
        public Dead(PlayerStateVariableContainer psvc) : base(psvc)
        {
            stateName = "Hurt";
            playerState = PlayerStateEnum.Dead;
        }

        protected override void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast)
        {
            return;
        }

        protected override void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            return;
        }
    }
}
