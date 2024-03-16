using System;
using EnemyScripts.AttackData;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieAnimator : MonoBehaviour
    {
        private ZombieScript _zombieScript;
        private Animator _animator;

        #region Variables

        private bool _isAttacking;
        private bool _isBlocking;
        private bool _isDead;
        private bool _isHurt;
        private bool _isPlayerCloseEnough;
        private bool _isStunned;
        private float _steppingSpeed;
        private float _hurtDirection;
        private int _attackStage;
        private int _attackType;
        private bool _isPlayerDead;
        private int _guardDirection;

        struct AnimatorVariables
        {
            public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
            public static readonly int IsBlocking = Animator.StringToHash("IsBlocking");
            public static readonly int IsDead = Animator.StringToHash("IsDead");
            public static readonly int IsHurt = Animator.StringToHash("IsHurt");
            public static readonly int IsStunned = Animator.StringToHash("IsStunned");
            public static readonly int IsPlayerCloseEnough = Animator.StringToHash("IsPlayerCloseEnough");
            public static readonly int SteppingSpeed = Animator.StringToHash("SteppingSpeed");
            public static readonly int HurtDirection = Animator.StringToHash("HurtDirection");
            public static readonly int AttackStage = Animator.StringToHash("AttackStage");
            public static readonly int AttackType = Animator.StringToHash("AttackType");
            public static readonly int IsPlayerDead = Animator.StringToHash("IsPlayerDead");
            public static readonly int GuardDirection = Animator.StringToHash("GuardDirection");
        }

        #endregion
        

        #region UnityEvents

        private void Awake()
        {
            _zombieScript = GetComponentInParent<ZombieScript>();
            _animator = GetComponent<Animator>();

        }

        private void OnEnable()
        {
            _zombieScript.onStateChange += ProcessAction_onStateChange;
            _zombieScript.onMovementSpeedChange += ProcessAction_onMovementSpeedChange;
            _zombieScript.onAttackProcessChange += ProcessAction_onAttackProcessChange;
            _zombieScript.onAttackTypeChange += ProcessAction_onAttackTypeChange;
            _zombieScript.onHurting += ProcessAction_onHurting;
            _zombieScript.onSettingNewZombieGuardPosition += ProcessAction_onSettingNewZombieGuardPosition;
        }

        private void OnDisable()
        {
            _zombieScript.onStateChange -= ProcessAction_onStateChange;
            _zombieScript.onMovementSpeedChange -= ProcessAction_onMovementSpeedChange;
            _zombieScript.onAttackProcessChange -= ProcessAction_onAttackProcessChange;
            _zombieScript.onAttackTypeChange -= ProcessAction_onAttackTypeChange;
            _zombieScript.onHurting -= ProcessAction_onHurting;
            _zombieScript.onSettingNewZombieGuardPosition -= ProcessAction_onSettingNewZombieGuardPosition;
        }
        
        

        #endregion

        #region Methods

        

        #endregion

        #region EventMethods

        void ProcessAction_onStateChange(ZombieStateEnum state)
        {
            switch (state)
            {
                case ZombieStateEnum.None:
                    break;
                case ZombieStateEnum.Spawn:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _isPlayerCloseEnough = false;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Standing:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _isPlayerCloseEnough = true;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.FollowPlayer:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _isPlayerCloseEnough = false;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Attacking:
                    _isAttacking = true;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _isPlayerCloseEnough = true;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Stunned:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _animator.SetTrigger(AnimatorVariables.IsStunned);
                    _isPlayerCloseEnough = false;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Hurt:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = true;
                    _isPlayerCloseEnough = true;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Dead:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = true;
                    _isHurt = false;
                    _isPlayerCloseEnough = true;
                    _isPlayerDead = false;
                    break;
                case ZombieStateEnum.Dancin:
                    _isAttacking = false;
                    _isBlocking = false;
                    _isDead = false;
                    _isHurt = false;
                    _isPlayerDead = true;
                    break;
                    
            }
            
            _animator.SetBool(AnimatorVariables.IsAttacking, _isAttacking);
            _animator.SetBool(AnimatorVariables.IsBlocking, _isBlocking);
            _animator.SetBool(AnimatorVariables.IsDead, _isDead);
            _animator.SetBool(AnimatorVariables.IsHurt, _isHurt);
            _animator.SetBool(AnimatorVariables.IsPlayerCloseEnough, _isPlayerCloseEnough);
            _animator.SetBool(AnimatorVariables.IsPlayerDead, _isPlayerDead);
        }

        void ProcessAction_onMovementSpeedChange(float steppingSpeed)
        {
            _steppingSpeed = steppingSpeed;
            _animator.SetFloat(AnimatorVariables.SteppingSpeed, _steppingSpeed);
        }

        void ProcessAction_onAttackTypeChange(AttackDirection type)
        {
            switch (type)
            {
                case AttackDirection.None:
                    break;
                case AttackDirection.DownAttack:
                    _attackType = 2;
                    break;
                case AttackDirection.LeftSlap:
                    _attackType = 0;
                    break;
                case AttackDirection.RightSlap:
                    _attackType = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            _animator.SetInteger(AnimatorVariables.AttackType, _attackType);
        }
        
        void ProcessAction_onAttackProcessChange(AttackProcess stage)
        {

            switch (stage)
            {
                case AttackProcess.WindUp:
                    _attackStage = 0;
                    
                    break;
                case AttackProcess.Attack:
                    _attackStage = 1;
                    
                    break;
                case AttackProcess.Hitbox:
                    _attackStage = 2;
                    
                    break;
                case AttackProcess.Recovery:
                    _attackStage = 3;
                    
                    break;
                case AttackProcess.Finished:
                    _attackStage = 4;
                    
                    break;
                default:
                    return;
            }
            
            _animator.SetInteger(AnimatorVariables.AttackStage, _attackStage);
        }

        void ProcessAction_onHurting(Vector3 hitDirection)
        {
            _hurtDirection = hitDirection.x;
            
            _animator.SetFloat(AnimatorVariables.HurtDirection, _hurtDirection);
        }

        void ProcessAction_onSettingNewZombieGuardPosition(ZombieGuardDirection direction)
        {
            switch (direction)
            {
                case ZombieGuardDirection.None:
                    break;
                    _guardDirection = 0;
                case ZombieGuardDirection.Up:
                    break;
                case ZombieGuardDirection.Down:
                    _guardDirection = 3;
                    break;
                case ZombieGuardDirection.Left:
                    _guardDirection = 2;
                    break;
                case ZombieGuardDirection.Right:
                    _guardDirection = 1;
                    break;
                case ZombieGuardDirection.EveryDirection:
                    _guardDirection = 4;
                    break;
            }
            
            _animator.SetFloat(AnimatorVariables.GuardDirection, _guardDirection);
        }

        #endregion

    }
}
