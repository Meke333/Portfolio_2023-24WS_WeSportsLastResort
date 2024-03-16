using System;
using PlayerScripts.Core;
using PlayerScripts.StateMachines.Player;
using UnityEngine;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerAnimator : PlayerSystem
    {
        #region Parameter
        
        private Animator _animator;
        private float _timeOfLastStep;
        private float _timeOfCurrentStep;

        [SerializeField] private bool _isStepping;
        [SerializeField] private float _currentStepTime;
        [SerializeField] private float _stepDirection;

        [SerializeField] private AnimationCurve stepSpeedCurve;

        struct AnimatorVariables
        {
            public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
            public static readonly int IsDefending = Animator.StringToHash("IsDefending");
            public static readonly int IsHurt = Animator.StringToHash("IsHurt");
            public static readonly int IsStunned = Animator.StringToHash("IsStunned");
            public static readonly int IsDead = Animator.StringToHash("IsDead");
            public static readonly int IsStepping = Animator.StringToHash("IsStepping");
            public static readonly int SteppingSpeed = Animator.StringToHash("SteppingSpeed");
            public static readonly int SteppingDirection = Animator.StringToHash("SteppingDirection");
        }

        private bool _isAttacking;
        private bool _isDefending;
        private bool _isHurt;
        //private bool _isStunned;
        //private bool _isStepping;
        private bool _isDead;
        private float _steppingSpeed;
        private float _steppingDirection;

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_isStepping)
                return;

            _currentStepTime += _currentStepTime < 1 ? Time.deltaTime * _steppingSpeed : 0;

            int stepOrigin = (int) -_stepDirection/2;
            
            //Lerp
            _steppingDirection = Mathf.Lerp((float)stepOrigin, (float)_stepDirection, _currentStepTime);
        
            
            _animator.SetFloat(AnimatorVariables.SteppingDirection, _steppingDirection);

            _isStepping = _currentStepTime < 1;

        }

        private void OnEnable()
        {
            PlayerEvents.onPlayerStateMachine_onStateChange += ProcessAction_onPlayerStateMachine_onStateChange;
            PlayerEvents.onMovement_TakingAStep += ProcessAction_onMovement_TakingAStep;
        }

        private void OnDisable()
        {
            PlayerEvents.onPlayerStateMachine_onStateChange -= ProcessAction_onPlayerStateMachine_onStateChange;
            PlayerEvents.onMovement_TakingAStep -= ProcessAction_onMovement_TakingAStep;
        }

        #endregion

        #region Methods
        

        
        #endregion

        #region EventMethods

        void ProcessAction_onPlayerStateMachine_onStateChange(PlayerStateEnum state)
        {
            switch (state)
            {
                case PlayerStateEnum.None:
                    break;
                case PlayerStateEnum.Idle:
                     _isAttacking = false;
                     _isDefending = false;
                     _isHurt = false; 
                     _isDead = false;
                    break;
                case PlayerStateEnum.Moving:
                     _isAttacking = false;
                     _isDefending = false;
                     _isHurt = false; 
                     _isDead = false;
                    break;
                case PlayerStateEnum.Attacking:
                     _isAttacking = true;
                     _isDefending = false;
                     _isHurt = false; 
                     _isDead = false;
                    break;
                case PlayerStateEnum.Blocking:
                     _isAttacking = false;
                     _isDefending = true;
                     _isHurt = false; 
                     _isDead = false;
                    break;
                case PlayerStateEnum.Stunned:
                     _isAttacking = false;
                     _isDefending = false;
                     _isHurt = false; 
                     
                    _animator.SetTrigger(AnimatorVariables.IsStunned);
                     _isDead = false;
                    break;
                case PlayerStateEnum.Hurt:
                     _isAttacking = false;
                     _isDefending = false;
                     _isHurt = true; 
                    _isDead = false;
                    break;
                case PlayerStateEnum.Dead:
                     _isAttacking = false;
                     _isDefending = false;
                     _isHurt = false; 
                     _isDead = true;
                    break;
            }
            
            _animator.SetBool(AnimatorVariables.IsAttacking, _isAttacking);
            _animator.SetBool(AnimatorVariables.IsDefending, _isDefending);
            _animator.SetBool(AnimatorVariables.IsHurt, _isHurt);
            _animator.SetBool(AnimatorVariables.IsDead, _isDead);
            
        }
//WIP TAKING A STEP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        void ProcessAction_onMovement_TakingAStep(int direction)
        {
            //Debug.Log("Step!");
            _isStepping = true;

            _currentStepTime = 0;

            _stepDirection = direction;
            
            //_animator.SetFloat(AnimatorVariables.SteppingDirection, direction);
            
            _animator.SetTrigger(AnimatorVariables.IsStepping);

            _timeOfLastStep = _timeOfCurrentStep;
            _timeOfCurrentStep = Time.time;

            float timeBetweenSteps = _timeOfCurrentStep - _timeOfLastStep;
            _steppingSpeed = stepSpeedCurve.Evaluate(timeBetweenSteps);
            _animator.SetFloat(AnimatorVariables.SteppingSpeed, _steppingSpeed);
        }

        #endregion
        
    }
}
