using System.Collections.Generic;
using EnemyScripts.EnemyStateMachine.Zombies;
using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using General.SingletonClass;
using UnityEngine;

namespace EnemyScripts.AttackData
{
    public class ProcessAttackAnimations : SingletonClass<ProcessAttackAnimations>
    {
        public Dictionary<AttackDirection, AttackAnimationStruct> ZombieAttackTime = new Dictionary<AttackDirection, AttackAnimationStruct>();

        public AttackAnimationStruct[] attackAnimations;

        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < attackAnimations.Length; i++)
            {
                AddInDictionary(attackAnimations[i]);
            }
        }

        void AddInDictionary(AttackAnimationStruct aAD)
        {
            if (ZombieAttackTime.ContainsKey(aAD.attackType))
                return;

            /*aAD.windUp_time = FramesInSeconds(aAD.windUp_frames, aAD.windUp_AttackAnimation.frameRate);
            aAD.attack_time = FramesInSeconds(aAD.attack_frames, aAD.attack_AttackAnimation.frameRate);
            aAD.recovery_time = FramesInSeconds(aAD.recovery_frames, aAD.recovery_AttackAnimation.frameRate);
            */

            aAD.windUp_time = aAD.windUp_AttackAnimation.length;
            aAD.windUp_frames = SecondsInFrames(aAD.windUp_time, aAD.windUp_AttackAnimation.frameRate);

            aAD.attack_time = aAD.attack_AttackAnimation.length;
            aAD.attack_frames = SecondsInFrames(aAD.attack_time, aAD.attack_AttackAnimation.frameRate);

            //Debug.LogError("hitBoxAnimation = " + aAD.hitbox_AttackAnimation.name);
            aAD.hitbox_time = aAD.hitbox_AttackAnimation.length;
            aAD.hitbox_frames = SecondsInFrames(aAD.hitbox_time, aAD.hitbox_AttackAnimation.frameRate);

            aAD.recovery_time = aAD.recovery_AttackAnimation.length;
            aAD.recovery_frames = SecondsInFrames(aAD.recovery_time, aAD.recovery_AttackAnimation.frameRate);
            
            
            //Debug.LogError("AttackAnimationType: " + aAD.attackType + "; attack_TIme: " + aAD.attack_time + "; hitbox_Time: " + aAD.hitbox_time);
            
            ZombieAttackTime.Add(aAD.attackType, aAD);
            
            
            
            
        }

        public AttackAnimationStruct GetFromDictionary(AttackDirection attackType)
        {
            return ZombieAttackTime[attackType];
        }

        float FramesInSeconds(int frames, float framerate)
        {
            return (frames / framerate);
        }

        int SecondsInFrames(float seconds, float framerate)
        {
            return (int)(framerate * seconds);
        }


    }

    [System.Serializable]
    public struct AttackAnimationStruct
    {
        public AttackDirection attackType;
        
        public AnimationClip windUp_AttackAnimation;
        public AnimationClip attack_AttackAnimation;
        public AnimationClip hitbox_AttackAnimation;
        public AnimationClip recovery_AttackAnimation;

        public int windUp_frames;
        public int attack_frames;
        public int hitbox_frames;
        public int recovery_frames;

        public float windUp_time;
        public float attack_time;
        public float hitbox_time;
        public float recovery_time;

    }

    public enum AttackProcess
    {
        WindUp,
        Attack,
        Hitbox,
        Recovery,
        Finished,
    }
}
