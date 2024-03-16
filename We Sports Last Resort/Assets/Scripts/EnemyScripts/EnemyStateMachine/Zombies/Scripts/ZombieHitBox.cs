using System;
using Core;
using Effects;
using General.Helper;
using Interface;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieHitBox : MonoBehaviour
    {
        private ZombieScript zombieScript;

        #region Variables

        
        [SerializeField] private LayerMask _hurtBoxLayerMask;
        [SerializeField] private Transform leftHandHitboxTransform;
        [SerializeField] private Transform rightHandHitboxTransform;
        private BoxCollider leftHandHitboxCollider;
        private BoxCollider rightHandHitboxCollider; 
        private bool _isHitboxActive;

        private Vector3 _attackDirection;
        
        private Timer _particleTimer;
        private bool _isParticleTriggered;
        private readonly float _particleCooldown = 1f;

        #endregion

        #region UnityMethods

        protected virtual void Awake()
        {
            zombieScript = transform.GetComponentInParent<ZombieScript>();

            leftHandHitboxCollider = leftHandHitboxTransform.GetComponentInChildren<BoxCollider>();
            rightHandHitboxCollider = rightHandHitboxTransform.GetComponentInChildren<BoxCollider>();

            _particleTimer = new Timer(_particleCooldown);
            _particleTimer.onTimerDone += () => _isParticleTriggered = false;
        }

        private void OnEnable()
        {
            zombieScript.onToggleHitbox += ProcessAction_onToggleHitbox;
            zombieScript.onAttackTypeChange += ProcessAction_onAttackTypeChange;
        }

        private void OnDisable()
        {
            zombieScript.onToggleHitbox -= ProcessAction_onToggleHitbox;
            zombieScript.onAttackTypeChange -= ProcessAction_onAttackTypeChange;
        }

        private void Update()
        {
            if (!_isHitboxActive)
                return;

            EvaluateHitbox();
        }

        #endregion
        

        #region Methods

        void EvaluateHitbox()
        {
            int maxColliders = 2;

            Collider[] leftHitColliders = new Collider[maxColliders];
            Collider[] rightHitColliders = new Collider[maxColliders];
            int numberOfLeftColliders = Physics.OverlapBoxNonAlloc(
                leftHandHitboxCollider.center + leftHandHitboxTransform.position, 
                leftHandHitboxCollider.size,
                leftHitColliders, 
                leftHandHitboxTransform.rotation, 
                _hurtBoxLayerMask);
            
            int numberOfRightColliders = Physics.OverlapBoxNonAlloc(
                rightHandHitboxCollider.center + rightHandHitboxTransform.position, 
                rightHandHitboxCollider.size,
                rightHitColliders, 
                rightHandHitboxTransform.rotation, 
                _hurtBoxLayerMask);
            
            if (numberOfLeftColliders + numberOfRightColliders <= 0)
            {
                //Debug.Log("No Hurtboxes Collided!");
                return;
            }

            //Debug.Log("Hurtboxes Collided!!!! " + numberOfLeftColliders + numberOfRightColliders);
            
            for (int i = 0; i < maxColliders; i++)
            {
                if (leftHitColliders[i] != null)
                {
                    IHurtbox iHurtbox = leftHitColliders[i].GetComponent<IHurtbox>();
                    if (iHurtbox != null)
                    {
                        if (iHurtbox.IsGettingBlocked(_attackDirection) == BlockReaction.Blocked)
                        {
                            GettingBlocked();
                            continue;
                        }

                        HittingTarget(iHurtbox, leftHitColliders[i].transform, Vector3.zero);
                    }
                }

                if (rightHitColliders[i] != null)
                {
                    IHurtbox iHurtbox = rightHitColliders[i].GetComponent<IHurtbox>();
                    if (iHurtbox != null)
                    {
                        if (iHurtbox.IsGettingBlocked(_attackDirection) == BlockReaction.Blocked)
                        {
                            GettingBlocked();
                            continue;
                        }

                        HittingTarget(iHurtbox, rightHitColliders[i].transform, Vector3.zero);

                    }
                }
            }


        }

        void GettingBlocked()
        {
            Debug.LogWarning("PLAYER BLOCKED ATTACK!!!");
            //Get into Stunned State!
            zombieScript.onTriggerStunState?.Invoke();
            _isHitboxActive = false;
            
            //BlockParticleSpawn
            ParticleEffectManager.Instance.PlayParticleEffect(gameObject.transform,ParticleEnum.Sweat);
            return;
            
        }

        void HittingTarget(IHurtbox target,Transform position, Vector3 direction)
        {
            target.GetHit(direction);
            
            if (_isParticleTriggered)
                return;

            _isParticleTriggered = true;
            _particleTimer.ResetAndRunTimer();
            
            Debug.LogWarning("PLAY PUNCH SHOUND");
            ParticleEffectManager.Instance.PlayParticleEffect(position, ParticleEnum.AttackHit);
            CoreEventManager.Instance.AudioEvents.OnZombie_Punch?.Invoke(zombieScript.ZombieNumber);
        }

        #endregion

        #region EventMethods

        void ProcessAction_onToggleHitbox(bool value)
        {
            _isHitboxActive = value;
        }

        void ProcessAction_onHurting(Vector3 a)
        {
            _isHitboxActive = false;
        }

        void ProcessAction_onAttackTypeChange(AttackDirection direction)
        {
            switch (direction)
            {
                case AttackDirection.None:
                    break;
                case AttackDirection.DownAttack:
                    _attackDirection = Vector2.down;
                    break;
                case AttackDirection.LeftSlap:
                    _attackDirection = Vector2.left;
                    break;
                case AttackDirection.RightSlap:
                    _attackDirection = Vector2.right;
                    break;
            }
        }

        #endregion
    }

    public enum AttackDirection
    {
        None,
        DownAttack,
        LeftSlap,
        RightSlap,
    }
}
