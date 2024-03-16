using General.StateBase;
using PlayerScripts.StateMachines.Player.States;
using UnityEngine;

namespace PlayerScripts.StateMachines.Player
{
    [System.Serializable]
    public abstract class PlayerStateMachine : StateBase
    {
        
        public PlayerStateVariableContainer playerStateVariableContainer { get; protected set; }

        #region Variables
        
        protected PlayerStateEnum playerState;
        
        #endregion

        public PlayerStateMachine(PlayerStateVariableContainer psvc)
        {
            playerStateVariableContainer = psvc;
        }

        public override void Enter()
        {
            base.Enter();
            
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_onStateChange?.Invoke(playerState);

            #region Events

            playerStateVariableContainer.PlayerEvents.onWiiMote_IsYawFast += ProcessAction_onWiiMote_IsYawOrPitchFast;
            playerStateVariableContainer.PlayerEvents.onWiiMote_IsPitchFast += ProcessAction_onWiiMote_IsYawOrPitchFast;
            playerStateVariableContainer.PlayerEvents.onWiiMote_GetButtons += ProcessAction_onWiiMote_GetButtons;

            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_TriggerStunState += ProcessAction_onPlayerStateMachine_TriggerStunState;
            playerStateVariableContainer.PlayerEvents.onPlayerStateMachine_TakingDamage += ProcessAction_onPlayerStateMachine_TakingDamage;

            #endregion

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }

        #region EventMethods

        protected abstract void ProcessAction_onWiiMote_IsYawOrPitchFast(bool isFast);

        protected virtual void ProcessAction_onWiiMote_GetButtons(bool[][] buttons)
        {
            playerStateVariableContainer.Buttons = buttons;
            
            //implementation in inherited classes!
        }

        protected virtual void ProcessAction_onPlayerStateMachine_TriggerStunState()
        {
            nextState = new Stunned(playerStateVariableContainer);
            stage = EVENT.EXIT;
        }

        protected virtual void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            //Debug.Log("Taking Damage: " + (playerStateVariableContainer.PlayerScript.GetHealth() > 0) );
            nextState = playerStateVariableContainer.PlayerScript.GetHealth() > 0 
                ? new Hurt(playerStateVariableContainer)
                : new Dead(playerStateVariableContainer);
            
            stage = EVENT.EXIT;
        }
        
        
        
        #endregion

    }
}
