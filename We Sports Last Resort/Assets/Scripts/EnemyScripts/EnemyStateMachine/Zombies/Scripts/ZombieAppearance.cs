using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieAppearance : MonoBehaviour
    {
        #region Parameters

        private ZombieScript _zombieScript;

        [Header("MeshRenderer")] 
        [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer leftHandMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer rightHandMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer leftLegMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer rightLegMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer earsMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer headMeshRenderer;
        
        [Space]
        [Header("Materials")]
        [SerializeField] private Material[] swapBodyMat;
        [SerializeField] private Material[] swapLeftHandMat;
        [SerializeField] private Material[] swapRightHandMat;
        [SerializeField] private Material[] swapLeftLegMat;
        [SerializeField] private Material[] swapRightLegMat;
        [SerializeField] private Material[] swapEarsMat;
        [SerializeField] private Material[] swapHeadMat;


        #endregion

        private void Awake()
        {
            _zombieScript = GetComponent<ZombieScript>();
        }

        private void OnEnable()
        {
            _zombieScript.onReset += ProcessAction_onReset;
        }

        private void OnDisable()
        {
            _zombieScript.onReset -= ProcessAction_onReset;
        }

        void ProcessAction_onReset()
        {
            int materialIndex = 0;

            materialIndex = Random.Range(0, 3);
            bodyMeshRenderer.material = swapBodyMat[materialIndex];
            
            materialIndex = Random.Range(0, 3);
            earsMeshRenderer.material = swapEarsMat[materialIndex];
            headMeshRenderer.material = swapHeadMat[materialIndex];
            leftHandMeshRenderer.material = swapLeftHandMat[materialIndex];
            rightHandMeshRenderer.material = swapRightHandMat[materialIndex];
            
            materialIndex = Random.Range(0, 3);
            leftLegMeshRenderer.material = swapLeftLegMat[materialIndex];
            rightLegMeshRenderer.material = swapRightLegMat[materialIndex];
            
            
        }
        
    }
}
