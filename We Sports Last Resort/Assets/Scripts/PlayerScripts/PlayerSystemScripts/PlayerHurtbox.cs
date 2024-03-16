using System;
using Core;
using Effects;
using General.Helper;
using Interface;
using PlayerScripts.Core;
using PlayerScripts.StateMachines.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerHurtbox : PlayerSystem, IHurtbox
    {
        private bool _canGetHit = true;
        private readonly float _coolDownTime = 1f;
        private Timer _coolDownUntilAnotherHit;

        [SerializeField] private PlayerStateEnum playerStateEnum;
        [FormerlySerializedAs("swordTransform")] [SerializeField] private Transform swordZeroPointTransform;
        [FormerlySerializedAs("hitBoxTransform")] [SerializeField] private Transform swordTransform;

        public Transform blockingParticlePosition;
        
        public Vector3 swordDirection;
        public Vector3 zombieHitDirection;
        public string attackName;
        
        protected override void Awake()
        {
            base.Awake();

            _coolDownUntilAnotherHit = new Timer(_coolDownTime);
            _coolDownUntilAnotherHit.onTimerDone += ProcessAction_coolDownUntilAnotherHit_onTimerDone;
        }

        private void Update()
        {
            swordDirection = (swordTransform.position - swordZeroPointTransform.position).normalized;
        }

        private void OnEnable()
        {
            PlayerEvents.onPlayerStateMachine_onStateChange += ProcessAction_onPlayerStateMachine_onStateChange;
        }

        private void OnDisable()
        {
            PlayerEvents.onPlayerStateMachine_onStateChange -= ProcessAction_onPlayerStateMachine_onStateChange;
        }


        public void GetHit(Vector3 hitDirection)
        {
            if (!_canGetHit)
                return;

            _canGetHit = false;
            _coolDownUntilAnotherHit.RunTimer();
            
            //Take Damage Player
            PlayerScript.TakeDamage();
            //Events
        }

        public BlockReaction IsGettingBlocked(Vector3 hitDirection)
        {
            //needs Guarding Direction -> Position of the Wii Mote
            BlockReaction result = BlockReaction.Hit;

            zombieHitDirection = hitDirection;

            if (playerStateEnum != PlayerStateEnum.Blocking)
                return BlockReaction.Hit;

            swordDirection = (swordTransform.position - swordZeroPointTransform.position).normalized;
            float xSwordDirection = Mathf.Abs(swordDirection.x);
            float ySwordDirection = Mathf.Abs(swordDirection.y);


            Debug.Log("ZombieHitDirection: " + hitDirection);
            
            float xHitDirection = Mathf.Abs(hitDirection.x);
            float yHitDirection = Mathf.Abs(hitDirection.y);

            if (yHitDirection > xHitDirection)
            {
                //Downward Smash
                attackName = "Downward Smash";
                
                if (ySwordDirection < xSwordDirection)//Mathf.Abs(swordDirection.x) > 0.75f)
                {
                    //Horizontal Block
                    attackName += " with Horizontal Block";
                    result = BlockReaction.Blocked;
                }
                
            }
            else if (xHitDirection > yHitDirection)
            {
                //Sidewards Attack
                attackName = "Sidewards Attack";
                
                
                
                if (xSwordDirection < ySwordDirection)//Mathf.Abs(swordDirection.y) > 0.75f)
                {
                    //Vertical Block
                    attackName += " with vertical Block";
                    result = BlockReaction.Blocked;
                }
            }
            
            Debug.Log("PlayerHurtbox: " + attackName);

            if (result == BlockReaction.Blocked)
            {
                ParticleEffectManager.Instance.PlayParticleEffect(blockingParticlePosition, ParticleEnum.Blocking);
               
                //AUDIOEVENT
                CoreEventManager.Instance.AudioEvents.OnWeapon_Block?.Invoke();
            }
            
            return result;
        }
        
        void ProcessAction_coolDownUntilAnotherHit_onTimerDone()
        {
            _canGetHit = true;
            _coolDownUntilAnotherHit.ResetTimer();
        }

        void ProcessAction_onPlayerStateMachine_onStateChange(PlayerStateEnum pse)
        {
            playerStateEnum = pse;
        }
        
    }
}
