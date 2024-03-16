using System.Threading.Tasks;
using Core;
using EnemyScripts.EnemyStateMachine.Zombies.States;
using General.StateBase;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies
{
    [System.Serializable]
    public class ZombieStateMachine : StateBase
    {

        [SerializeField] protected ZombieStateEnum _type;
        protected bool isHurt;


        [SerializeField] public ZombieStateVariableContainer zombieStateVariableContainer; //{ get; protected set; }

        public ZombieStateMachine(ZombieStateVariableContainer zsvc)
        {
            zombieStateVariableContainer = zsvc;
        }
        
        public override async void Enter()
        {
            base.Enter();
            while (zombieStateVariableContainer.ZombieScript == null)
            {
                await Task.Yield();
            }
            
            zombieStateVariableContainer.ZombieScript.onStateChange?.Invoke(_type);

            zombieStateVariableContainer.ZombieScript.onHurting += ProceedToHurt;
            zombieStateVariableContainer.ZombieScript.onTriggerStunState += ProceedToStun;
            await Task.Yield();
            CoreEventManager.Instance.GameEvents.OnPlayerDied += ProceedToDance;
        }

        public override async void Update()
        {
            base.Update();

            while (zombieStateVariableContainer == null)
            {
                await Task.Yield();
            }

            if (zombieStateVariableContainer.isFinishedTransmittingData || !zombieStateVariableContainer.ZombieScript.isZombieFocused)
                return;
            
            if (zombieStateVariableContainer.GetDistanceFromEnemyToTarget() >= zombieStateVariableContainer.FocusingDistanceToPlayer)
            {
                zombieStateVariableContainer.isFinishedTransmittingData = false;
                return;
            }

            zombieStateVariableContainer.isFinishedTransmittingData = true;

            zombieStateVariableContainer.ZombieScript.SignalPlayerNewTarget();
        }

        public override void Exit()
        {
            
            zombieStateVariableContainer.ZombieScript.onHurting -= ProceedToHurt;
            zombieStateVariableContainer.ZombieScript.onTriggerStunState -= ProceedToStun;
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= ProceedToDance;
            
            base.Exit();
        }

        public virtual void ProceedToHurt(Vector3 hitDirection)
        {
            //Debug.Log("PROCEEDTOHURT");
            isHurt = true;
            zombieStateVariableContainer.ZombieScript.onCanMoveToPlayer?.Invoke(false);
            zombieStateVariableContainer.ZombieScript.onCanRotateToPlayer?.Invoke(false);

            zombieStateVariableContainer.ZombieScript.onToggleHitbox?.Invoke(false);
            Debug.Log("Health left: " + zombieStateVariableContainer.Health);
            zombieStateVariableContainer.hitDirection = hitDirection;
            
            if (!zombieStateVariableContainer.ZombieScript.isZombieFocused || zombieStateVariableContainer.Health > 0)
            {
                
                zombieStateVariableContainer.ZombieScript.CheckIfThisZombieIsFocused();
                if (_type != ZombieStateEnum.Hurt)
                {
                    nextState = new Hurt(zombieStateVariableContainer);
                    
                    stage = EVENT.EXIT;
                }
            }
            else
            {
                nextState = new Dead(zombieStateVariableContainer);

                stage = EVENT.EXIT;
            }

        }

        public virtual void ProceedToStun()
        {
            Debug.Log("PROCEEDTOSTUN");
            nextState = new Stunned(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }

        public virtual void ProceedToDance()
        {
            if (_type == ZombieStateEnum.Dead)
                return;

            nextState = new Dancin(zombieStateVariableContainer);
            stage = EVENT.EXIT;
        }

    }
}
