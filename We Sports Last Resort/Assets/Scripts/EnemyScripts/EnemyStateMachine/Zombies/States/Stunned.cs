using General.Helper;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Stunned : ZombieStateMachine
    {
        private float _stunTime = 2f;
        private Timer _stunnedTimer;

        public Stunned(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Stunned";
            _type = ZombieStateEnum.Stunned;

            _stunnedTimer = new Timer(_stunTime);
            _stunnedTimer.onTimerDone += ProcessAction_stunnedTimer_onTimerDone;
        }

        public override void Enter()
        {
            _stunnedTimer.RunTimer();
            base.Enter();
        }

        void ProcessAction_stunnedTimer_onTimerDone()
        {
            //Go back to Idle State
            
            Debug.Log("Stun finished");
            nextState = new Standing(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }

        public override void ProceedToStun()
        {
            base.ProceedToStun();
            
            _stunnedTimer.InterruptTimer();
            
        }

        public override void ProceedToHurt(Vector3 hitDirection)
        {
            base.ProceedToHurt(hitDirection);
            
            _stunnedTimer.InterruptTimer();
        }
    }
}
