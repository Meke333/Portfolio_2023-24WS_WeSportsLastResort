using System.Threading.Tasks;
using Core;
using EnemyScripts;
using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using Level;
using PlayerScripts.Core;
using PlayerScripts.StateMachines.Player;
using UnityEngine;
using UnityEngine.Splines;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerGroundMovement : PlayerSystem
    {
        private SplineContainer _levelPath;
        private float _speed = 1;
        private float _pathPercentage;

        private float _splineLength;
        private bool _isLevelFinished;

        private Vector3 _target;
        private bool _isTargetMoved;
        private float _targetTime;
        private Quaternion _targetLookDirection;

        private ZombieScript _targetZombie;
        private Transform _targetZombieTransform;
        [SerializeField] private bool _isZombieAlive;
        private readonly float _minimumDistanceToZombie = 2.25f;

        [SerializeField] private bool isPathLoopable;

        private Rigidbody _rb;

        [SerializeField] private Transform graphics;
        private float tilt;

        private int lastDirection = 1;

        private bool _isPlayerDead;

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();

            _rb = GetComponent<Rigidbody>();

            GetLevelPath();
        }

        private async void OnEnable()
        {
            PlayerEvents.onMovement_TakingAStep += ProcessAction_onMovement_TakingAStep;
            PlayerEvents.onMovement_MovingToSides += ProcessAction_onMovement_MovingToSides;
            PlayerEvents.onMovement_ChangeSpeed += ProcessAction_onChangingSpeed;
            PlayerEvents.onBalanceBoard_CenterOfGravity += ProcessAction_onBalanceBoard_CenterOfGravity;
            PlayerEvents.onEnemies_ZombieDied += ProcessAction_onEnemies_ZombieDied;
            PlayerEvents.onEnemies_FocusingZombie += ProcessAction_onEnemies_FocusingZombie;
            PlayerEvents.onPlayerStateMachine_TakingDamage += ProcessAction_onPlayerStateMachine_TakingDamage;
            PlayerEvents.onPlayerStateMachine_onStateChange += ProcessAction_onPlayerStateMachine_onStateChange;

            await Task.Yield();

            EnemyManagerScript.Instance.onAllZombiesAreDead += ProcessAction_onAllZombiesAreDead;
            CoreEventManager.Instance.GameEvents.OnPlayerDied += () => _isPlayerDead = true;

        }

        private void OnDisable()
        {
            PlayerEvents.onMovement_TakingAStep -= ProcessAction_onMovement_TakingAStep;
            PlayerEvents.onMovement_MovingToSides -= ProcessAction_onMovement_MovingToSides;
            PlayerEvents.onMovement_ChangeSpeed -= ProcessAction_onChangingSpeed;
            PlayerEvents.onBalanceBoard_CenterOfGravity -= ProcessAction_onBalanceBoard_CenterOfGravity;
            PlayerEvents.onEnemies_ZombieDied -= ProcessAction_onEnemies_ZombieDied;
            PlayerEvents.onEnemies_FocusingZombie -= ProcessAction_onEnemies_FocusingZombie;
            PlayerEvents.onPlayerStateMachine_TakingDamage -= ProcessAction_onPlayerStateMachine_TakingDamage;
            PlayerEvents.onPlayerStateMachine_onStateChange -= ProcessAction_onPlayerStateMachine_onStateChange;
            
            EnemyManagerScript.Instance.onAllZombiesAreDead -= ProcessAction_onAllZombiesAreDead;
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= () => _isPlayerDead = true;
        }

        private void Update()
        {
            _targetTime += _targetTime < 1 ? Time.deltaTime * 2 : 0;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastDirection *= -1;
                PlayerEvents.onMovement_TakingAStep(lastDirection);
                //ProcessAction_onMovement_TakingAStep(1);
            }
        }

        private void FixedUpdate()
        {
            if (_isPlayerDead)
                return;
            
            if (!_isTargetMoved)
                return;

            if(_isZombieAlive)
                LookAtZombie();
            
            _rb.MovePosition(Vector3.Lerp(transform.position, _target, _targetTime));
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, _targetLookDirection, _targetTime));
        }

        #endregion

        #region Methods

        void GetLevelPath()
        {
            _levelPath = LevelPathSingleton.Instance.levelPath;
            _splineLength = _levelPath.CalculateLength();
        }

        void LookAtZombie()
        {
            if (!_isZombieAlive)
                return;

            Vector3 lookDirection = _targetZombieTransform.position - transform.position;
            lookDirection.y = transform.position.y;
            
            
            _targetLookDirection = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        void SplineMovement(float speed)
        {
            if (_pathPercentage < 1f)
            {
                _pathPercentage += (1 / _splineLength) * speed;
                return;
            }
            
            switch (isPathLoopable)
            {
                case true:
                    _pathPercentage = 0f;
                    return;
                case false:
                    //LEVEL COMPLETED!!!
                    //INVOKE LEVEL COMPLETE EVENT!!!
                        
                    CoreEventManager.Instance.GameEvents.OnLevel1Finished?.Invoke();
                    _isLevelFinished = true;
                        
                    return;
            }
            

        }

        float ZombieFocusedSplineMovementSpeed()
        {
            if (!_isZombieAlive)
                return _speed;

            Vector3 targetZombiePosition = _targetZombieTransform.position;
            
            Vector3 currentPosition = _levelPath.EvaluatePosition(_pathPercentage);
            currentPosition.y = _target.y;
            
            Vector3 nextPosition = _levelPath.EvaluatePosition(_pathPercentage + 0.01f);
            nextPosition.y = _target.y;

            
            

            float currentDistanceToZombie =
                Vector3.Distance(currentPosition, targetZombiePosition) - _minimumDistanceToZombie;
            
            float nextDistanceToZombie = 
                Vector3.Distance(nextPosition, targetZombiePosition) - _minimumDistanceToZombie;
            
            if (currentDistanceToZombie < nextDistanceToZombie)
                return -_speed / 20;
            
            return Mathf.Min(currentDistanceToZombie, _speed);

        }

        #endregion

        #region EventMethods

        void ProcessAction_onMovement_TakingAStep(int direction)
        {
            //Taking step on Spline
            if(_isLevelFinished)
                return;
            
            CoreEventManager.Instance.GameEvents.OnPlayerTakingAStep?.Invoke();

            float moveSpeed =  ZombieFocusedSplineMovementSpeed();
            
            SplineMovement(moveSpeed);
            
            _isTargetMoved = true;
            _targetTime = 0;
            
            _target = _levelPath.EvaluatePosition(_pathPercentage);
            _target.y = transform.position.y;
            
            //AUDIOEVENT
            CoreEventManager.Instance.AudioEvents.OnPlayer_Footstep?.Invoke();
            
            if (_isZombieAlive)
                return;
            
            Vector3 nextPosition = _levelPath.EvaluatePosition(_pathPercentage + 0.01f);
            nextPosition.y = _target.y;
            
            Vector3 lookDirection = nextPosition - _target;
            
            _targetLookDirection = Quaternion.LookRotation(lookDirection, transform.up);
        }

        void ProcessAction_onPlayerStateMachine_TakingDamage(float damage)
        {
            //Debug.Log("Taking Damage Player Ground Movement");
            //Stepping Back

            Vector3 currentLocation = transform.position;

            float newValue = _pathPercentage - (2 / _splineLength);
            _pathPercentage = newValue < 0 ? 0 : newValue;

            if (_pathPercentage > 1f)
            {
                switch (isPathLoopable)
                {
                    case true:
                        _pathPercentage = 0f;
                        break;
                    case false:
                        //LEVEL COMPLETED!!!
                        //INVOKE LEVEL COMPLETE EVENT!!!
                        
                        break;
                }

                return;
            }

            _isTargetMoved = true;
            _targetTime = 0;
            
            _target = _levelPath.EvaluatePosition(_pathPercentage);
            _target.y = transform.position.y;

            Vector3 lookDirection = _target - currentLocation;

            if (_isZombieAlive)
                return;
            
            _targetLookDirection = Quaternion.LookRotation(lookDirection, transform.up);
            
        }
        
        void ProcessAction_onMovement_MovingToSides(float directionSpeed)
        {
            //Moving to sides
            //tilt = -directionSpeed * 20;
            //graphics.localRotation = Quaternion.Euler(0, tilt, tilt);
        }

        void ProcessAction_onBalanceBoard_CenterOfGravity(Vector2 cog)
        {
            tilt = cog.x;
            graphics.localRotation = Quaternion.Euler(0, tilt, tilt);
        }

        void ProcessAction_onEnemies_FocusingZombie(ZombieScript zombie)
        {
            
            _targetTime = 0;
            
            Debug.Log("Player: FocusingZombie");
            _targetZombie = zombie;
            _targetZombieTransform = _targetZombie.transform;
            _isZombieAlive = true;

        }

        void ProcessAction_onChangingSpeed(float newSpeed)
        {
            _speed = newSpeed;
            //0.05f
        }

        void ProcessAction_onEnemies_ZombieDied()
        {
            //Debug.Log("Player: Zombie Died");
            _targetZombie = null;
            _targetZombieTransform = null;
            _isZombieAlive = false;
        }

        void ProcessAction_onAllZombiesAreDead()
        {
            Debug.Log("YOU CAN MOVE ON!!!");
            ProcessAction_onMovement_TakingAStep(0);
            _speed = 1;
        }

        void ProcessAction_onPlayerStateMachine_onStateChange(PlayerStateEnum state)
        {
            if (state == PlayerStateEnum.Dead)
            {
                
            }
        }

        #endregion
        
    }
}
