using System;
using Core;
using PlayerScripts.Core;
using UnityEngine;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerRagdoll : PlayerSystem
    {
        [Header("Ragdoll")] [SerializeField] private Rigidbody[] ragdollRigidbodies;

        enum RagdollState
        {
            Deactive,
            Active
        }

        [SerializeField] private RagdollState _ragdollState;

        [Header("Other Components")]
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;

        [Header("Dead Stuff")] 
        [SerializeField] private Material playerHeadMaterial;
        [SerializeField] private Material playerBodyMaterial;

        [SerializeField] private GameObject[] playerHeadGameObjects;
        [SerializeField] private GameObject[] playerBodyGameObjects;
        [SerializeField] private GameObject playerLeftHandGameObject;
        [SerializeField] private GameObject playerRightHandGameObject;

        #region UnityEvents

        protected override void Awake()
        {
            base.Awake();

            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

            _animator = GetComponent<Animator>();
            
            
            DisableRagdoll();
        }


        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnPlayerDied += ProcessAction_GameEvents_OnPlayerDied;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= ProcessAction_GameEvents_OnPlayerDied;
        }

        #endregion

        #region Methods

        void EnableRagdolls()
        {
            _ragdollState = RagdollState.Active;

            for (int i = 0; i < ragdollRigidbodies.Length; i++)
            {
                ragdollRigidbodies[i].isKinematic = false;
            }

            _animator.enabled = false;
            _characterController.enabled = false;
        }

        void DisableRagdoll()
        {
            _ragdollState = RagdollState.Deactive;

            for (int i = 0; i < ragdollRigidbodies.Length; i++)
            {
                ragdollRigidbodies[i].isKinematic = true;
            }

            _animator.enabled = true;
            _characterController.enabled = true;
        }

        #endregion

        #region EventMethods

        void ProcessAction_GameEvents_OnPlayerDied()
        {
            EnableRagdolls();

            int headLength = playerHeadGameObjects.Length;
            int bodyLength = playerBodyGameObjects.Length;

            int checkLength = headLength > bodyLength ? headLength : bodyLength;

            for (int i = 0; i < checkLength; i++)
            {
                if (i < headLength)
                {
                    playerHeadGameObjects[i].GetComponent<SkinnedMeshRenderer>().material = playerHeadMaterial;
                }

                if (i < bodyLength)
                {
                    playerBodyGameObjects[i].GetComponent<SkinnedMeshRenderer>().material = playerBodyMaterial;
                }

                playerLeftHandGameObject.GetComponent<SphereCollider>().enabled = true;
                playerRightHandGameObject.GetComponent<SphereCollider>().enabled = true;

            }
            
        }

        #endregion

    }
}
