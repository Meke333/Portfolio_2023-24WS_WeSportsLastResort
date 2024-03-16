using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class FollowPlayer : ZombieStateMachine
    {
        
        public FollowPlayer(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "FollowPlayer";
            _type = ZombieStateEnum.FollowPlayer;
        }

        public override void Enter()
        {
            base.Enter();
            
            zombieStateVariableContainer.ZombieScript.onCanMoveToPlayer?.Invoke(true);
            zombieStateVariableContainer.ZombieScript.onCanRotateToPlayer?.Invoke(true);
            
            //Debug.Log(zombieStateVariableContainer.ZombieScript.isZombieFocused);
        }

        public override void Update()
        {
            base.Update();

            MoveToPlayer();
            IsZombieCloseEnoughToPlayer();
        }

        public override void Exit()
        {
            
            zombieStateVariableContainer.ZombieScript.onCanMoveToPlayer?.Invoke(false);
            //zombieStateVariableContainer.ZombieScript.onCanRotateToPlayer?.Invoke(false);
            base.Exit();
            
            //Debug.LogWarning("OJJJAJAJ: " + (zombieStateVariableContainer.GetDistanceFromEnemyToTarget()));
            
        }

        void MoveToPlayer()
        {
            if (!zombieStateVariableContainer.CanMove)
                return;

            Vector3 direction = GetNormalizedDirectionVectorFromEnemyToTarget();

            if (zombieStateVariableContainer.GetDistanceFromEnemyToTarget() 
                >= zombieStateVariableContainer.DistanceBetweenPlayerAndEnemy)
            {
                //Events (Animator)
                
                //Events (ZombieMovement)
                zombieStateVariableContainer.ZombieScript.onMovingToPlayer?.Invoke(direction);
            }
            
        }

        
        Vector3 GetNormalizedDirectionVectorFromEnemyToTarget()
        {
            return (zombieStateVariableContainer.playerPosition.position - zombieStateVariableContainer.zombiePosition.position).normalized;
        }


        void IsZombieCloseEnoughToPlayer()
        {
            if (stage == EVENT.EXIT)
                return;
            
            if ((zombieStateVariableContainer.GetDistanceFromEnemyToTarget() >= zombieStateVariableContainer.DistanceBetweenPlayerAndEnemy)) 
                return;

            //Events (Stop Moving -> Idle)
            nextState = new Standing(zombieStateVariableContainer);
            stage = EVENT.EXIT;

        }
        
    }
}
