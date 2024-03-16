using General.Helper;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Hurt : ZombieStateMachine
    {
        private Timer _timerUntilRecovery;
        private readonly float _time = 1f;
        
        public Hurt(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Hurt";
            _type = ZombieStateEnum.Hurt;

            _timerUntilRecovery = new Timer(_time);
            _timerUntilRecovery.onTimerDone += ProcessAction_timerUntilRecovery_onTimerDone;
            _timerUntilRecovery.RunTimer();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            isHurt = false;
            base.Exit();
        }
        
        void ProcessAction_timerUntilRecovery_onTimerDone()
        {
            //Switch into Standing
            nextState = new Standing(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }
        
        public override void ProceedToHurt(Vector3 hitDirection)
        {
            base.ProceedToHurt(hitDirection);
            
            _timerUntilRecovery.ResetAndRunTimer();
        }
    }
}
