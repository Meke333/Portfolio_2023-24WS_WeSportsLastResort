using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieRagdoll : MonoBehaviour
    {
        [Header("Head")]
        public CharacterJoint headCharacterJoint;
        public GameObject headGameObject;
        public Rigidbody headRigidBody;
        struct CharacterJointData
        {
            public float breakForce;
            public float projectionAngle;
            public float projectionDistance;
            public Vector3 swingAxis;
            public Vector3 connectedAnchor;
            public bool autoConfigureConnectedAnchor;
            public Rigidbody connectedBody;
            public SoftJointLimitSpring twistLimitSpring;
            public SoftJointLimit lowTwistLimit;
            public SoftJointLimit highTwistLimit;
            public SoftJointLimitSpring swingLimitSpring;
            public SoftJointLimit swing1Limit;
            public SoftJointLimit swing2Limit;
            public bool enableProjection;
            public float breakTorque;
            public bool enableCollision;
            public bool enablePreprocessing;
            public float massScale;
            public float connectedMassScale;
            public ArticulationBody connectedArticulationBody;

        }
        private CharacterJointData _headCharacterJointData;

       

        public float forceNeededForHeadSplit;
        [Space]
        
        [Header("Ragdoll")]
        [SerializeField] private Rigidbody[] _ragdollRigidbodies;
        [SerializeField] private Vector3[] _ragdollPositions;
        [SerializeField] private Quaternion[] _ragdollRotations;

        private Vector3 _knockBackDirection;

        private Rigidbody _zombieRigidbody;
        enum RagdollState
        {
            Deactive,
            Active
        }
        [SerializeField] private RagdollState _ragdollState;


        [Space] 
        
        [Header("Other Components")] 
        private CharacterController _characterController;

        private Animator _animator;
        
        [Space]

        private ZombieScript _zombieScript;

        #region UnityEvents

        private void Awake()
        {
            _zombieScript = GetComponent<ZombieScript>();
            
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _zombieRigidbody = GetComponent<Rigidbody>();
            
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            
            
            GetLocalRagdollTransforms();

        }

        private void Start()
        {
            DisableRagdoll();

            //head CharacterJoint
            _headCharacterJointData.breakForce = headCharacterJoint.breakForce;
            _headCharacterJointData.breakTorque = headCharacterJoint.breakTorque;
            _headCharacterJointData.projectionAngle = headCharacterJoint.projectionAngle;
            _headCharacterJointData.projectionDistance = headCharacterJoint.projectionDistance;
            _headCharacterJointData.swingAxis = headCharacterJoint.swingAxis;
            _headCharacterJointData.connectedAnchor = headCharacterJoint.connectedAnchor;
            _headCharacterJointData.autoConfigureConnectedAnchor = headCharacterJoint.autoConfigureConnectedAnchor;
            _headCharacterJointData.connectedBody = headCharacterJoint.connectedBody;
            _headCharacterJointData.twistLimitSpring = headCharacterJoint.twistLimitSpring;
            _headCharacterJointData.lowTwistLimit = _headCharacterJointData.lowTwistLimit;
            _headCharacterJointData.highTwistLimit = headCharacterJoint.highTwistLimit;
            _headCharacterJointData.swingLimitSpring = headCharacterJoint.swingLimitSpring;
            _headCharacterJointData.swing1Limit = headCharacterJoint.swing1Limit;
            _headCharacterJointData.swing2Limit = headCharacterJoint.swing2Limit;
            _headCharacterJointData.enableProjection = headCharacterJoint.enableProjection;
            _headCharacterJointData.breakTorque = headCharacterJoint.breakForce;
            _headCharacterJointData.enableCollision = headCharacterJoint.enableCollision;
            _headCharacterJointData.enablePreprocessing = headCharacterJoint.enablePreprocessing;
            _headCharacterJointData.massScale = headCharacterJoint.massScale;
            _headCharacterJointData.connectedMassScale = headCharacterJoint.connectedMassScale;
            _headCharacterJointData.connectedArticulationBody = headCharacterJoint.connectedArticulationBody;


        }

        private void OnEnable()
        {
            _zombieScript.onSplittingOffHead += SplitOffHead;
            _zombieScript.onSettingRagdoll += ProcessAction_onSettingRagdoll;
            _zombieScript.onResettingRagdoll += ResetRagdoll;
            _zombieScript.onTakingKnockback += value => _knockBackDirection = value;

        }

        private void OnDisable()
        {
            _zombieScript.onSplittingOffHead -= SplitOffHead;
            _zombieScript.onSettingRagdoll -= ProcessAction_onSettingRagdoll;
            _zombieScript.onResettingRagdoll -= ResetRagdoll;
            _zombieScript.onTakingKnockback -= value => _knockBackDirection = value;


        }


        #endregion
        
        #region Methods

        void SplitOffHead(Vector3 forceDirection)
        {
            if (headCharacterJoint == null)
                return;
            
            EnableRagdoll();
            
            Debug.Log("Force Direction Magnitude: " + forceDirection.magnitude);
            if (forceDirection.magnitude < forceNeededForHeadSplit)
                return;
            
            headCharacterJoint.breakForce = 0;
            
        }

        void AddHead()
        {
            if (headCharacterJoint != null)
                return;
            
            headCharacterJoint = headGameObject.AddComponent<CharacterJoint>();
            
            headCharacterJoint.breakForce = _headCharacterJointData.breakForce;
            headCharacterJoint.breakTorque = _headCharacterJointData.breakTorque;
            headCharacterJoint.projectionAngle = _headCharacterJointData.projectionAngle;
            headCharacterJoint.projectionDistance = _headCharacterJointData.projectionDistance;
            headCharacterJoint.swingAxis = _headCharacterJointData.swingAxis;
            headCharacterJoint.connectedAnchor = _headCharacterJointData.connectedAnchor;
            headCharacterJoint.autoConfigureConnectedAnchor = _headCharacterJointData.autoConfigureConnectedAnchor;
            headCharacterJoint.connectedBody = _headCharacterJointData.connectedBody;
            headCharacterJoint.twistLimitSpring = _headCharacterJointData.twistLimitSpring;
            headCharacterJoint.lowTwistLimit = _headCharacterJointData.lowTwistLimit;
            headCharacterJoint.highTwistLimit = _headCharacterJointData.highTwistLimit;
            headCharacterJoint.swingLimitSpring = _headCharacterJointData.swingLimitSpring;
            headCharacterJoint.swing1Limit = _headCharacterJointData.swing1Limit;
            headCharacterJoint.swing2Limit = _headCharacterJointData.swing2Limit;
            headCharacterJoint.enableProjection = _headCharacterJointData.enableProjection;
            headCharacterJoint.breakTorque = _headCharacterJointData.breakForce;
            headCharacterJoint.enableCollision = _headCharacterJointData.enableCollision;
            headCharacterJoint.enablePreprocessing = _headCharacterJointData.enablePreprocessing;
            headCharacterJoint.massScale = _headCharacterJointData.massScale;
            headCharacterJoint.connectedMassScale = _headCharacterJointData.connectedMassScale;
            headCharacterJoint.connectedArticulationBody = _headCharacterJointData.connectedArticulationBody;
        }
        
        void EnableRagdoll()
        {
            _ragdollState = RagdollState.Active;

            for (int i = 0; i < _ragdollRigidbodies.Length; i++)
            {
                _ragdollRigidbodies[i].isKinematic = false;
            }

            _animator.enabled = false;
            _characterController.enabled = false;

            _zombieRigidbody.isKinematic = true;
        }
        
        void DisableRagdoll()
        {
            _ragdollState = RagdollState.Deactive;
        
            for (int i = 0; i < _ragdollRigidbodies.Length; i++)
            {
                _ragdollRigidbodies[i].isKinematic = true;
            }
        
            _animator.enabled = true;
            _characterController.enabled = true;
        
            _zombieRigidbody.isKinematic = false;
        }

        void ResetRagdoll()
        {
            DisableRagdoll();
            SetLocalRagdollTransforms();
            AddHead();
        }

        void GetLocalRagdollTransforms()
        {
            int amount = _ragdollRigidbodies.Length;
            _ragdollPositions = new Vector3[amount];
            _ragdollRotations = new Quaternion[amount];

            for (int i = 0; i < amount; i++)
            {
                _ragdollPositions[i] = _ragdollRigidbodies[i].gameObject.transform.localPosition;
                _ragdollRotations[i] = _ragdollRigidbodies[i].gameObject.transform.localRotation;
            }

        }

        void SetLocalRagdollTransforms()
        {
            int amount = _ragdollRigidbodies.Length;

            for (int i = 0; i < amount; i++)
            {
                _ragdollRigidbodies[i].gameObject.transform.localPosition = _ragdollPositions[i];
                _ragdollRigidbodies[i].gameObject.transform.localRotation = _ragdollRotations[i];
            }
        }

        #endregion

        #region EventMethods

        void ProcessAction_onSettingRagdoll(bool isRagdollActive)
        {
            Debug.LogWarning("Activate ProcessAction_OnSettingRagdoll");
            switch (isRagdollActive)
            {
                case true:

                    if (_zombieScript.enemyHealth > 0)
                        return;
                    
                    for (int i = 0; i < _ragdollRigidbodies.Length; i++)
                    {
                        _ragdollRigidbodies[i].AddForce(_knockBackDirection, ForceMode.Impulse);
                    }
                    EnableRagdoll();
                    break;
                case false:

                    DisableRagdoll();

                    break;
            }
        }
        

        #endregion
    }
}
