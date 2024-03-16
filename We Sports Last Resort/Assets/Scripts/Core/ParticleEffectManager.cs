using System;
using System.Collections.Generic;
using General.Helper;
using General.SingletonClass;
using UnityEngine;
using UnityEngine.Serialization;

namespace Effects
{
    public class ParticleEffectManager : SingletonClass<ParticleEffectManager>
    {
        [System.Serializable]
        public struct ParticleSystemStruct
        {
            public GameObject particleGameObject;
            public ParticleEnum particleType;
            public ParticleSystem particleSystem;
        }

        public ParticleSystemStruct[] particleSystemArray;
        
        private Dictionary<ParticleEnum, ParticleSystemStruct> _particleDictionary =
            new Dictionary<ParticleEnum, ParticleSystemStruct>();

        #region Particle Pooling
        
        [Header("Particle Properties")]
        [Space]
        
        [SerializeField] private ParticleSystemStruct[] HitParticleEffects;
        [SerializeField] private int currentHitParticle = 0;
        [SerializeField] private int maxHitParticles;//STAYS SERIALIZEFIELD
        [SerializeField] private Transform hitGameParent;//STAYS SERIALIZEFIELD
        
        [Space]

        [SerializeField] private ParticleSystemStruct[] BlockedHitParticleEffects;
        [SerializeField] private int currentBlockedHitParticle = 0;
        [SerializeField] private int maxBlockedHitParticles;//STAYS SERIALIZEFIELD
        [SerializeField] private Transform blockedHitParent;//STAYS SERIALIZEFIELD
        
        [Space]

        [SerializeField] private ParticleSystemStruct[] SweatParticleEffects;
        [SerializeField] private int currentSweatParticle = 0;
        [SerializeField] private int maxSweatParticles;//STAYS SERIALIZEFIELD
        [SerializeField] private Transform sweatParent;//STAYS SERIALIZEFIELD
        
        [Space]

        [SerializeField] private ParticleSystemStruct[] GroundParticleEffects;
        [SerializeField] private int currentGroundParticle = 0;
        [SerializeField] private int maxGroundParticles;//STAYS SERIALIZEFIELD
        [SerializeField] private Transform groundParent;//STAYS SERIALIZEFIELD

        #endregion
        

        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < particleSystemArray.Length; i++)
            {
                ParticleSystemStruct temp =  particleSystemArray[i];
                
                _particleDictionary.Add(temp.particleType, temp);
                
            }

            #region Particle Pooling

            int[] maxNumbers = {
                    maxHitParticles, 
                    maxBlockedHitParticles, 
                    maxSweatParticles, 
                    maxGroundParticles
                    
            };

            int boundary;
            boundary = FindMaxInt.FindMaxIntOfArray(maxNumbers);
            if (boundary < 1)
                return;
            
            HitParticleEffects = new ParticleSystemStruct[maxHitParticles];
            BlockedHitParticleEffects = new ParticleSystemStruct[maxBlockedHitParticles];
            SweatParticleEffects = new ParticleSystemStruct[maxSweatParticles];
            GroundParticleEffects = new ParticleSystemStruct[maxGroundParticles];

            for (int i = 0; i < boundary; i++)
            {
                if (i < maxHitParticles)
                {
                    HitParticleEffects[i] = InstantiateParticleStruct(ParticleEnum.AttackHit, hitGameParent);
                    HitParticleEffects[i].particleGameObject.SetActive(false);
                }

                if (i < maxBlockedHitParticles)
                {
                    BlockedHitParticleEffects[i] = InstantiateParticleStruct(ParticleEnum.Blocking, blockedHitParent);
                    BlockedHitParticleEffects[i].particleGameObject.SetActive(false);
                }

                if (i < maxSweatParticles)
                {
                    SweatParticleEffects[i] = InstantiateParticleStruct(ParticleEnum.Sweat, sweatParent);
                    SweatParticleEffects[i].particleGameObject.SetActive(false);
                }

                if (i < maxGroundParticles)
                {
                    GroundParticleEffects[i] = InstantiateParticleStruct(ParticleEnum.Ground, groundParent);
                    GroundParticleEffects[i].particleGameObject.SetActive(false);
                }
                
            }
            
            

            #endregion

        }

        public void PlayParticleEffect(Transform transform, ParticleEnum particleType)
        {

            //InstantiateParticleEffect_Old(transform, particleType);

            
            switch (particleType)
            {
                case ParticleEnum.AttackHit:
                    currentHitParticle = PlayParticleFromArray(HitParticleEffects, currentHitParticle, transform);
                    break;
                case ParticleEnum.Blocking:
                    currentBlockedHitParticle = PlayParticleFromArray(BlockedHitParticleEffects, currentBlockedHitParticle, transform);
                    break;
                case ParticleEnum.Sweat:
                    currentSweatParticle = PlayParticleFromArray(SweatParticleEffects, currentSweatParticle, transform);
                    break;
                case ParticleEnum.Blood:
                    break;
                case ParticleEnum.Ground:
                    currentGroundParticle = PlayParticleFromArray(GroundParticleEffects, currentGroundParticle, transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(particleType), particleType, null);
            }
        }

        ParticleSystemStruct InstantiateParticleStruct(ParticleEnum type, Transform parent)
        {
            ParticleSystemStruct temp = new ParticleSystemStruct();

            temp.particleType = type;
            temp.particleGameObject = Instantiate(_particleDictionary[type].particleGameObject, parent);
            temp.particleSystem = temp.particleGameObject.GetComponent<ParticleSystem>();

            return temp;

        }
        public void InstantiateParticleEffect_Old(Transform transform, ParticleEnum particleType)
        {
            GameObject activatedGameObject;
            ParticleSystem activatedParticleSystem;
            

            activatedGameObject =
                Instantiate(_particleDictionary[particleType].particleGameObject, transform.position, transform.rotation);

            activatedParticleSystem = activatedGameObject.GetComponent<ParticleSystem>();
            
            activatedParticleSystem.Play();
        }

        int PlayParticleFromArray(ParticleSystemStruct[] particleArray, int currentIndex, Transform transform)
        {
            particleArray[currentIndex].particleGameObject.transform.position = transform.position;
            particleArray[currentIndex].particleGameObject.transform.rotation = transform.rotation;
            particleArray[currentIndex].particleGameObject.transform.localScale = transform.localScale;
            
            particleArray[currentIndex].particleGameObject.SetActive(true);
            particleArray[currentIndex].particleSystem.Play();
            currentIndex = (currentIndex+1) % particleArray.Length;

            return currentIndex;
        }
        
    }

    public enum ParticleEnum
    {
        AttackHit,
        Blocking,
        Sweat,
        Blood,
        Ground,
    }
}