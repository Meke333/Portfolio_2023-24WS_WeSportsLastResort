using System;
using Core;
using Core.EventManagers;
using Effects;
using General.Helper;
using Interface;
using PlayerScripts.Core;
using UnityEngine;
using WiiScripts.Input;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerWeaponSword : PlayerSystem
    {
        #region Parameter

        private bool _isWiiMoteActive;

        [SerializeField] private LayerMask _hurtBoxLayerMask;

        private Transform _zeroPoint;
        private Transform _actualSword;

        private Transform _hitbox;
        private BoxCollider _boxHitbox;

        public Transform TESTINGBLOCK;

        private bool _isHitboxActive;

        [SerializeField] private Vector3 _yawPitchRoll;

        [SerializeField] private Vector3 idlePosition;
        [SerializeField] private Vector3 attackPosition;
        [SerializeField] private Vector3 blockPosition;

        [SerializeField] private Transform sweatParticlePosition;

        private Timer _particleTimer;
        private bool _isParticleTriggered;
        private readonly float _particleCooldown = 0.5f;
        

        #endregion

        #region ReadonlyParameters

        private readonly float _mediumHitThreshold = 10f;
        private readonly float _strongHitThreshold = 15f;

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            
            _zeroPoint = this.transform;
            _actualSword = _zeroPoint.GetChild(0);
            _hitbox = _actualSword.GetChild(0);
            _boxHitbox = _hitbox.GetComponent<BoxCollider>();

            idlePosition = _actualSword.localPosition;

            #region Events
            
            ProcessAction_onWiiMote_IsWiiMoteActive(WiiMoteInput.Instance.isMotionPlusRemoteActive);
            
            /*
            PlayerEvents.onWiiMote_IsWiiMoteActive += ProcessAction_onWiiMote_IsWiiMoteActive;

            PlayerEvents.onWiiMote_GetMotionPlus += ProcessAction_onWiiMote_GetMotionPlus;
            PlayerEvents.onWiiMote_GetButtons += ProcessAction_onWiiMote_GetButtons;
            
            PlayerEvents.onPlayerStateMachine_isAttacking += ProcessAction_onPlayerStateMachine_isAttacking;
            PlayerEvents.onPlayerStateMachine_isDefending += ProcessAction_onPlayerStateMachine_isDefending;*/

            #endregion

            _particleTimer = new Timer(_particleCooldown);
            _particleTimer.onTimerDone += () => _isParticleTriggered = false;

        }

        private void OnEnable()
        {
            PlayerEvents.onWiiMote_IsWiiMoteActive += ProcessAction_onWiiMote_IsWiiMoteActive;

            PlayerEvents.onWiiMote_GetMotionPlus += ProcessAction_onWiiMote_GetMotionPlus;
            PlayerEvents.onWiiMote_GetButtons += ProcessAction_onWiiMote_GetButtons;
            
            PlayerEvents.onPlayerStateMachine_isAttacking += ProcessAction_onPlayerStateMachine_isAttacking;
            PlayerEvents.onPlayerStateMachine_isDefending += ProcessAction_onPlayerStateMachine_isDefending;

            CoreEventManager.Instance.GameEvents.OnPlayerDied += ProcessAction_GameEvents_OnPlayerDied;
        }

        private void OnDisable()
        {
            PlayerEvents.onWiiMote_IsWiiMoteActive -= ProcessAction_onWiiMote_IsWiiMoteActive;

            PlayerEvents.onWiiMote_GetMotionPlus -= ProcessAction_onWiiMote_GetMotionPlus;
            PlayerEvents.onWiiMote_GetButtons -= ProcessAction_onWiiMote_GetButtons;
            
            PlayerEvents.onPlayerStateMachine_isAttacking -= ProcessAction_onPlayerStateMachine_isAttacking;
            PlayerEvents.onPlayerStateMachine_isDefending -= ProcessAction_onPlayerStateMachine_isDefending;
            
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= ProcessAction_GameEvents_OnPlayerDied;
        }

        private void Update()
        {
            if (!_isHitboxActive)
                return;

            EvaluateHitbox();
        }

        #endregion

        #region Methods

        void ResetOffset()
        {
            _zeroPoint.localRotation = Quaternion.identity;
        }
        
        void EvaluateHitbox()
        {
            Vector3 direction = new Vector3(_yawPitchRoll.y,-_yawPitchRoll.x,transform.forward.z * 3);
            int maxColliders = 15;
            
            Collider[] hitColliders = new Collider[maxColliders];

            TESTINGBLOCK.position = _hitbox.position;
            TESTINGBLOCK.localScale = _boxHitbox.size;
            TESTINGBLOCK.rotation = _hitbox.rotation;
            
            //_hitbox.position + _boxHitbox.center was incorrect apparently ??????
            int numberOfColliders = Physics.OverlapBoxNonAlloc(_hitbox.position, 
                (_boxHitbox.size), 
                hitColliders, 
                _hitbox.rotation, 
                _hurtBoxLayerMask);

            if (numberOfColliders <= 0)
            {
                //Debug.Log("No Hurtboxes Collided!");
                return;
            }
            //Debug.Log("THERE ARE HURTBOXES");
            for (int i = 0; i < numberOfColliders; i++)
            {
                //Debug.Log("Hurtbox: " + hitColliders[i].gameObject.name);
                if (hitColliders[i].transform.root == PlayerScript.transform)
                    continue;

                IHurtbox iHurtbox = hitColliders[i].GetComponent<IHurtbox>();

                //Debug.Log("iHurtbox == null: " + (iHurtbox == null));
                if (iHurtbox == null)
                    continue;
                
                if (iHurtbox.IsGettingBlocked(direction) == BlockReaction.Blocked)
                {
                    GettingHit();
                    
                    //AUDIOEVENT
                    CoreEventManager.Instance.AudioEvents.OnWeapon_Block?.Invoke();
                    
                    continue;
                }
                
                //Debug.Log("Processing Getting Hit");
                HitTarget(iHurtbox, hitColliders[i].transform, direction);
            }
        }

        void GettingHit()
        {
            _isHitboxActive = false;
            Debug.LogWarning("ENEMY BLOCKED ATTACK!!!");
            //Get into Stunned State!
            PlayerEvents.onPlayerStateMachine_TriggerStunState?.Invoke();
            
            if (_isParticleTriggered)
                return;

            _isParticleTriggered = true;
            _particleTimer.ResetAndRunTimer();
            
            ParticleEffectManager.Instance.PlayParticleEffect(PlayerScript.PlayerData.sweatParticle, ParticleEnum.Sweat);
            
        }

        void HitTarget(IHurtbox target, Transform position, Vector3 direction)
        {
            target?.GetHit(direction);
            
            if (_isParticleTriggered)
                return;

            _isParticleTriggered = true;
            _particleTimer.ResetAndRunTimer();
            

            Vector3 targetPosition = position.position;
            
            Vector3 directionFromTargetToPlayer;
            directionFromTargetToPlayer = gameObject.transform.position - targetPosition;
            
            Vector3 vectorPosition = (directionFromTargetToPlayer * 0.05f) + targetPosition;
            vectorPosition.y = _actualSword.position.y;


            Transform newPosition = position;
            newPosition.position = vectorPosition;
            
            
            ParticleEffectManager.Instance.PlayParticleEffect(newPosition, ParticleEnum.AttackHit);

            DetermineSwingStrength(direction);
        }

        void DetermineSwingStrength(Vector3 force)
        {
            float damage = Vector3.Magnitude(force);

            HitStrength result = HitStrength.None;
            if (damage < _mediumHitThreshold)
                result = HitStrength.Weak;
            
            else if (damage < _strongHitThreshold)
                result = HitStrength.Medium;
            
            else
                result = HitStrength.Strong;
            
            Debug.Log("DetermineSwingStrength: " + damage);
            
            //AUDIOEVENT
            CoreEventManager.Instance.AudioEvents.OnWeapon_Hit?.Invoke(result);

        }
        

        #endregion

        #region EventMethods

        void ProcessAction_onWiiMote_IsWiiMoteActive(bool isActive)
        {
            _isWiiMoteActive = isActive;
        }
        
        void ProcessAction_onWiiMote_GetMotionPlus(Vector3 motionPlus)
        {
            _yawPitchRoll = motionPlus;
            _yawPitchRoll.y = -_yawPitchRoll.y;
            //_yawPitchRoll.z = 0;
            
            _zeroPoint.Rotate(_yawPitchRoll);
        }
        
        void ProcessAction_onPlayerStateMachine_isAttacking(bool isAttacking)
        {
            _isHitboxActive = isAttacking;
            
            //Extend the Sword
            switch (isAttacking)
            {
                case true:

                    _actualSword.localPosition = attackPosition;
                    
                    break;
                case false:
                    
                    _actualSword.localPosition = idlePosition;
                    
                    break;
            }
            
        }

        void ProcessAction_onPlayerStateMachine_isDefending(bool isBlocking)
        {
            //Tuck in the Sword
            switch (isBlocking)
            {
                case true:

                    _actualSword.localPosition = blockPosition;
                    
                    break;
                case false:

                    _actualSword.localPosition = idlePosition;
                    
                    break;
            }
        }

        void ProcessAction_onWiiMote_GetButtons(bool[][] buttons)
        {
            bool isResetOffsetPressed =
                buttons[0][1] || buttons[4][1] || buttons[5][1] || buttons[6][1] || buttons[7][1];
            
            if (isResetOffsetPressed) //[A or D-Pad][GetButtonDown]
            {
                ResetOffset();
            }

        }

        void ProcessAction_GameEvents_OnPlayerDied()
        {
            _actualSword.gameObject.AddComponent<Rigidbody>();
            _actualSword.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }

        #endregion
    }
}
