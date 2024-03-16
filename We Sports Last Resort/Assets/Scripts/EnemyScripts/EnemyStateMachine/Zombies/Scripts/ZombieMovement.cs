using System;
using PlayerScripts.Core;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieMovement : MonoBehaviour
    {
        private ZombieScript zombieScript;

        private bool _canMove;
        private bool _canRotate;

        private Vector3 _direction;

        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private float _speed;

        [Header("Knockback")]
        private float _deadKnockbackStrength = 5;
        [SerializeField] private Vector3 _knockBackDirection;
        [SerializeField] private AnimationCurve _knockbackCurve;
        [SerializeField] private float _smoothKnockbackStrength;
        

        protected virtual void Awake()
        {
            zombieScript = transform.GetComponentInParent<ZombieScript>();

            _rigidbody = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
            _speed = zombieScript.movementSpeed;
        }

        private void OnEnable()
        {
            zombieScript.onCanMoveToPlayer += value => _canMove = value;
            zombieScript.onCanRotateToPlayer += value => _canRotate = value;
            zombieScript.onMovingToPlayer += value => _direction = value;

            zombieScript.onSettingNewPosition += SetPosition;
            zombieScript.onTakingKnockback += TakingKnockBack;

        }

        private void OnDisable()
        {
            zombieScript.onCanMoveToPlayer -= value => _canMove = value;
            zombieScript.onCanRotateToPlayer -= value => _canRotate = value;
            zombieScript.onMovingToPlayer -= value => _direction = value;
            
            zombieScript.onSettingNewPosition -= SetPosition;
            zombieScript.onTakingKnockback -= TakingKnockBack;

        }

        private void Update()
        {
            SmoothKnockback();
        }

        private void FixedUpdate()
        {
            
            MoveToPlayer();
            RotateToPlayer();
            
        }

        void MoveToPlayer()
        {
            //if (!_canMove)
               // return;
            
            if (!_characterController.enabled)
                return;
            
            //Will later be replaced with root motion
            //_rigidbody.MovePosition(_direction.normalized * _speed + transform.position);
            //_characterController.SimpleMove(_direction.normalized * _speed);

            float value = _knockbackCurve.Evaluate(_smoothKnockbackStrength);
            //Debug.Log(value);
            _characterController.SimpleMove( _knockBackDirection * value /*Vector3.zero*/);
        }

        void RotateToPlayer()
        {
            if (!_canRotate)
                return;
            
            Vector3 a = PlayerScript.Instance.transform.position;
            a.y = zombieScript.transform.position.y;
            transform.LookAt(a);
        }

        void SmoothKnockback()
        {
            _smoothKnockbackStrength += _smoothKnockbackStrength < 1 ? Time.deltaTime * 2.5f : 0;
        }

        void TakingKnockBack(Vector3 dir)
        {
            //Debug.Log("TakingKnockback");

            _smoothKnockbackStrength = 0;
            _knockBackDirection = (-dir.normalized - transform.forward) * 0.5f;
            //_characterController.SimpleMove(dir * (zombieScript.enemyHealth <= 0 ? _deadKnockbackStrength : 3 ));
        }

        async void SetPosition(Vector3 position)
        {
            _characterController.enabled = false;
            _rigidbody.MovePosition(position);

            await Task.Delay(250);
            
            _characterController.enabled = true;
        }
        
        
    }
}
