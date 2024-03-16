using General.Helper;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Dead : ZombieStateMachine
    {
        private Timer _timerToDespawn;
        private readonly float _time = 1.5f;
        
        public Dead(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Dead";
            _type = ZombieStateEnum.Dead;

            _timerToDespawn = new Timer(_time);
            _timerToDespawn.onTimerDone += ProcessAction_timerToDespawn_onTimerDone;
        }

        public override void Enter()
        {
            base.Enter();
            _timerToDespawn.RunTimer();
            
            Debug.Log("Invoking Death: " + zombieStateVariableContainer.ZombieScript.name);
            zombieStateVariableContainer.ZombieScript.DeathTrigger();
            
            zombieStateVariableContainer.ZombieScript.onSplittingOffHead?.Invoke(zombieStateVariableContainer.hitDirection);
            zombieStateVariableContainer.ZombieScript.onSettingRagdoll?.Invoke(true);
        }

        private void ProcessAction_timerToDespawn_onTimerDone()
        {
            zombieStateVariableContainer.ZombieScript.DeathDespawn();
        }

        public override void ProceedToHurt(Vector3 hitDirection)
        {
            
        }

        public override void ProceedToStun()
        {
            
        }
    }
}
