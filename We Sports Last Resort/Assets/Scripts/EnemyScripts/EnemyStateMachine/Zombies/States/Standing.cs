using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using General.Helper;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Standing : ZombieStateMachine
    {
        private Timer _timerUntilAttack;
        private readonly float _time;


        public Standing(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Standing";
            _type = ZombieStateEnum.Standing;
            
            _time = Random.Range(0.1f, 1);

            

            _timerUntilAttack = new Timer(_time);
            _timerUntilAttack.onTimerDone += ProcessAction__timerUntilAttack_onTimerDone;
            _timerUntilAttack.RunTimer();

            
            
            
        }

        public override void Update()
        {
            base.Update();

            if (isHurt 
                || !(zombieStateVariableContainer.GetDistanceFromEnemyToTarget()
                     > zombieStateVariableContainer.DistanceBetweenPlayerAndEnemy)) 
                return;
            
            nextState = new FollowPlayer(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }

        public override void Exit()
        {
            base.Exit();
            //Debug.LogWarning("IUPIP: " + (zombieStateVariableContainer.GetDistanceFromEnemyToTarget()));
        }

        void ProcessAction__timerUntilAttack_onTimerDone()
        {
            if (!zombieStateVariableContainer.ZombieScript.isZombieFocused)
            {
                _timerUntilAttack.ResetAndRunTimer();
                return;
            }
            
            //Establish Attack!

            AttackDirection temp;
            
            int minInclude = 0;
            int maxExclude = 3;
            int i = Random.Range(minInclude, maxExclude);
            
            switch (i)
            {
                case 0:
                    temp = AttackDirection.LeftSlap;
                    break;
                case 1:
                    temp = AttackDirection.RightSlap;
                    break;
                case 2:
                    temp = AttackDirection.DownAttack;
                    break;
                default:
                    temp = AttackDirection.None;
                    break;
            }
            
            zombieStateVariableContainer.AttackDirection = temp;
            
            zombieStateVariableContainer.ZombieScript.onAttackTypeChange?.Invoke(temp);
            
            //Switch into Attack
            nextState = new Attack(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }

        public override void ProceedToHurt(Vector3 hitDirection)
        {
            base.ProceedToHurt(hitDirection);

            _timerUntilAttack.InterruptTimer();
        }
    }
}
