using System;
using Core;
using Effects;
using General.Helper;
using Interface;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieHurtBox : MonoBehaviour,IHurtbox
    {
        [SerializeField] private float damageMultiplier = 1;
        private ZombieGuardDirection _zombieGuardDirection;
        
        private ZombieScript zombieScript;

        private bool _canGetHit = true;
        private readonly float _coolDownTime = 0.5f;
        private Timer _coolDownUntilAnotherHit;
        
        private Timer _particleTimer;
        private bool _isParticleTriggered;
        private readonly float _particleCooldown = 0.5f;

        [SerializeField] private ZombieStateEnum zombieState;

        [SerializeField] private Transform shield;

        [SerializeField] private CapsuleCollider hurtCollider;
        [SerializeField] private CapsuleCollider pushCollider;
        
        
        protected void Awake()
        {
            zombieScript = transform.GetComponentInParent<ZombieScript>();
            
            _zombieGuardDirection = zombieScript.ZombieGuardDirection;
            
            _coolDownUntilAnotherHit = new Timer(_coolDownTime);
            _coolDownUntilAnotherHit.onTimerDone += ProcessAction_coolDownUntilAnotherHit_onTimerDone;
            
            _particleTimer = new Timer(_particleCooldown);
            _particleTimer.onTimerDone += () => _isParticleTriggered = false;
        }

        private void OnEnable()
        {
            zombieScript.onStateChange += value => zombieState = value;
            zombieScript.onSettingNewZombieGuardPosition += value => _zombieGuardDirection = value;
            zombieScript.onZombieDead += ProcessAction_onZombieDead;
            zombieScript.onReset += ProcessAction_onReset;
        }

        private void OnDisable()
        {
            zombieScript.onStateChange -= value => zombieState = value;
            zombieScript.onSettingNewZombieGuardPosition -= value => _zombieGuardDirection = value;
            zombieScript.onZombieDead -= ProcessAction_onZombieDead;
            zombieScript.onReset -= ProcessAction_onReset;
        }

        public void GetHit(Vector3 hitDirection)
        {
            //Debug.Log("Zombie Get Hit triggered!");
            if (!_canGetHit)
                return;
            //Debug.Log("Zombie Got Hit!");
            
            _canGetHit = false;
            _coolDownUntilAnotherHit.RunTimer();
            
            if (zombieState == ZombieStateEnum.Dead || zombieState == ZombieStateEnum.Spawn)
                return;

            float damage = Vector3.Magnitude(hitDirection) * damageMultiplier;

            TakeDamage(damage, hitDirection);

            zombieScript.onHurting?.Invoke(hitDirection);
            
            //AUDIOEVENT
            //CoreEventManager.Instance.AudioEvents.OnZombie_Hurt?.Invoke(zombieScript.ZombieNumber);
            
            
        }

        public BlockReaction IsGettingBlocked(Vector3 hitDirection)
        {
            if (!zombieScript.isZombieFocused)
            {
                return BlockReaction.Ignored;
            }
                
            BlockReaction result = BlockReaction.Hit;
            DirectionEnum directionEnum = DetermineDirectionInEnum.GetDirectionEnum(hitDirection);
            
            switch (_zombieGuardDirection)
            {
                case ZombieGuardDirection.None:
                    
                    break;
                case ZombieGuardDirection.Up:
                    //Debug.Log("Processing Direction?: " + directionEnum);
                    if (zombieState != ZombieStateEnum.Attacking && directionEnum == DirectionEnum.Up)
                        result = BlockReaction.Blocked;
                    
                    break;
                case ZombieGuardDirection.Down:
                    if (zombieState != ZombieStateEnum.Attacking && directionEnum == DirectionEnum.Down)
                        result = BlockReaction.Blocked;
                    
                    break;
                case ZombieGuardDirection.Left:
                    if (zombieState != ZombieStateEnum.Attacking && directionEnum == DirectionEnum.Left)
                        result = BlockReaction.Blocked;
                    
                    break;
                case ZombieGuardDirection.Right:
                    if (zombieState != ZombieStateEnum.Attacking && directionEnum == DirectionEnum.Right)
                        result = BlockReaction.Blocked;
                    
                    break;
                case ZombieGuardDirection.EveryDirection:
                    if (zombieState != ZombieStateEnum.Attacking && zombieState != ZombieStateEnum.Stunned)
                        result = BlockReaction.Blocked;

                    break;
            }

            /*if (!zombieScript.isZombieFocused && result == BlockReaction.Blocked)
            {
                result = BlockReaction.Ignored;
            }*/  //if you want Zombies to still take damage, but ignore any effects with a block

            if (zombieScript.isZombieFocused && !_isParticleTriggered && result == BlockReaction.Blocked)
            {
                _isParticleTriggered = true;
                _particleTimer.ResetAndRunTimer();
                ParticleEffectManager.Instance.PlayParticleEffect(shield, ParticleEnum.Blocking);
                
                //AUDIOEVENT
                CoreEventManager.Instance.AudioEvents.OnWeapon_Block?.Invoke();
            }
            
            return result;
        }

        void TakeDamage(float damage, Vector3 knockback)
        {
            zombieScript.TakeDamage(damage);
            zombieScript.onTakingKnockback?.Invoke(knockback);
        }

        void ProcessAction_coolDownUntilAnotherHit_onTimerDone()
        {
            _canGetHit = true;
            _coolDownUntilAnotherHit.ResetTimer();
        }

        void ProcessAction_onZombieDead()
        {
            hurtCollider.enabled = false;
            pushCollider.enabled = false;
        }

        void ProcessAction_onReset()
        {
            hurtCollider.enabled = true;
            pushCollider.enabled = true;
        }
    }
}
