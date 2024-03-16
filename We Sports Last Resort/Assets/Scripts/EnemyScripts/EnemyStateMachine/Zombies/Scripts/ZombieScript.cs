using System;
using System.Threading.Tasks;
using Core;
using Effects;
using EnemyScripts.AttackData;
using EnemyScripts.EnemyStateMachine.Zombies.States;
using PlayerScripts.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    [SelectionBase]
    public class ZombieScript : MonoBehaviour
    {
        #region Parameters

        public bool isStartedOnce;
        public bool isActive;
        
        public string enemyName;
        public float enemyHealth;
        public float movementSpeed;

        public ZombieGuardDirection ZombieGuardDirection;

        public bool isZombieFocused;

        private int _zombieNumber;

        public int ZombieNumber
        {
            get => _zombieNumber;
            set
            {
                _zombieNumber = value;
                onTransmitZombieNumber?.Invoke(_zombieNumber);
            }
        }

        #endregion

        #region ZombieEvents

        //Needs to be implemented
        public Action<bool> onIsZombieActive;
        public Action<int> onTransmitZombieNumber;

        public Action<string> onTransmittingNames;
        
        public Action<Vector3> onMovingToPlayer;
        public Action<bool> onCanMoveToPlayer;
        public Action<bool> onCanRotateToPlayer;
        public Action<float> onMovementSpeedChange;
        public Action<Vector3> onSettingNewPosition;

        public Action<ZombieStateEnum> onStateChange;

        public Action<Vector3> onHurting;
        public Action<float> onTakingDamage;
        public Action<Vector3> onTakingKnockback;

        public Action<bool> onToggleHitbox;
        public Action<AttackDirection> onAttackTypeChange;
        public Action<AttackProcess> onAttackProcessChange;

        public Action<ZombieGuardDirection> onSettingNewZombieGuardPosition;

        public Action<Vector3> onSplittingOffHead;
        public Action<bool> onSettingRagdoll;
        public Action onResettingRagdoll;
        public Action onReset;

        public Action onTriggerStunState;

        public Action onFocused;

        public Action onZombieDead;

        #endregion

        #region StateMachine

        [SerializeField] private ZombieStateMachine currentZombieState;

        #endregion

        #region UnityMethod

        private async void Start()
        {
            ZombieStateVariableContainer newVariableContainer =
                new ZombieStateVariableContainer(this, enemyHealth, movementSpeed);
            
            await Task.Yield();
            
            currentZombieState = new FollowPlayer(newVariableContainer);
            currentZombieState.zombieStateVariableContainer.Initialize();
            
            
            onMovementSpeedChange?.Invoke(movementSpeed);
        }

        private void OnDisable()
        {
            currentZombieState.zombieStateVariableContainer.ShutDown();
        }

        private void Update()
        {
            currentZombieState = (ZombieStateMachine) currentZombieState.Process();
        }

        #endregion

        #region Methods

        public void TakeDamage(float damage)
        {
            //Debug.Log("Taking Damage");
            enemyHealth -= damage;
            onTakingDamage?.Invoke(damage);

            if (enemyHealth > 0)
                return;
            
            if (!isZombieFocused)
                return;
            
            
            PlayerScript.Instance.PlayerEvents.onEnemies_ZombieDied?.Invoke();
        }

        public void ResetZombieScript(Transform newTransform, float newHealth, float newMovementSpeed, string newName, ZombieGuardDirection zombieGuardDirection)
        {
            onResettingRagdoll?.Invoke();
            onSettingNewPosition?.Invoke(newTransform.position);
            gameObject.transform.position = newTransform.position; 
            gameObject.transform.rotation = newTransform.rotation;
            
            //Transmit new Guard Direction
            ZombieGuardDirection = zombieGuardDirection;
            onSettingNewZombieGuardPosition?.Invoke(ZombieGuardDirection);
            
            //ParticleEffectManager
            ParticleEffectManager.Instance.PlayParticleEffect(newTransform, ParticleEnum.Ground);

            enemyHealth = newHealth;
            movementSpeed = newMovementSpeed;
            
            onMovementSpeedChange?.Invoke(movementSpeed);

            if (!isStartedOnce)
            {
                isStartedOnce = true;
                Start();
            }
            else
            {
                ZombieStateVariableContainer temp = currentZombieState.zombieStateVariableContainer;
                temp.Health = enemyHealth;
                temp.MoveSpeed = movementSpeed;
                temp.isFinishedTransmittingData = false;
                
                currentZombieState = new Standing(temp);
            }
            
            
            //onSettingNewZombieGuardPosition?.Invoke(ZombieGuardDirection); //Guard Position is sometimes bugged
            onReset?.Invoke();
            
            CoreEventManager.Instance.AudioEvents.OnZombie_Grunt?.Invoke(_zombieNumber);
        }

        public void SetActive(bool stage)
        {
            isActive = stage;
        }

        public void SetThisOneZombieAsFocus(bool value)
        {
            isZombieFocused = value;
            onFocused?.Invoke();

        }

        public void CheckIfThisZombieIsFocused()
        {
            EnemyManagerScript.Instance.RecheckIfZombieIsFocused();
        }

        public bool IsAlive()
        {
            return enemyHealth > 0;
        }

        public void SignalPlayerNewTarget()
        {
            if (!isZombieFocused)
                return;
            
            Debug.Log("Signaling Zombie?: " + this.gameObject.name);
            
            PlayerScript.Instance.PlayerEvents.onEnemies_FocusingZombie?.Invoke(this);
        }

        public void DeathTrigger()
        {
            EnemyManagerScript.Instance.ZombieDead(this);
            onZombieDead?.Invoke();
        }

        public void DeathDespawn()
        {
            gameObject.SetActive(false);
            isZombieFocused = false;
        }

        #endregion
    }
}
