using General.SingletonClass;
using UnityEngine;

namespace Core
{
    public class CoreAudioManager : SingletonClass<CoreAudioManager>
    {
        [SerializeField] private AudioClip[] playerFootstep;
        [SerializeField] private AudioClip[] zombieFootstep;
        [SerializeField] private AudioClip[] zombieGrunt;
        [SerializeField] private AudioClip[] zombieHurt;
        [SerializeField] private AudioClip[] zombiePunch;
        [SerializeField] private AudioClip[] weaponBlock;
        [SerializeField] private AudioClip[] weaponHitStrong;
        [SerializeField] private AudioClip[] weaponHitMedium;
        [SerializeField] private AudioClip[] weaponHitWeak;
        [SerializeField] private AudioClip[] tennisRacket;
        
        public enum SoundType
        {
            PlayerFootstep,
            ZombieFootstep,
            ZombieGrunt,
            ZombieHurt,
            ZombiePunch,
            WeaponBlock,
            WeaponHitStrong,
            WeaponHitMedium,
            WeaponHitWeak,
            TennisRacket
        }
        
        public AudioClip GetSound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.PlayerFootstep:
                    return playerFootstep[Random.Range(0, playerFootstep.Length)];
                case SoundType.ZombieFootstep:
                    return zombieFootstep[Random.Range(0, zombieFootstep.Length)];
                case SoundType.ZombieGrunt:
                    return zombieGrunt[Random.Range(0, zombieGrunt.Length)];
                case SoundType.ZombieHurt:
                    return zombieHurt[Random.Range(0, zombieHurt.Length)];
                case SoundType.ZombiePunch:
                    return zombiePunch[Random.Range(0, zombiePunch.Length)];
                case SoundType.WeaponBlock:
                    return weaponBlock[Random.Range(0, weaponBlock.Length)];
                case SoundType.WeaponHitStrong:
                    return weaponHitStrong[Random.Range(0, weaponHitStrong.Length)];
                case SoundType.WeaponHitMedium:
                    return weaponHitMedium[Random.Range(0, weaponHitMedium.Length)];
                case SoundType.WeaponHitWeak:
                    return weaponHitWeak[Random.Range(0, weaponHitWeak.Length)];
                case SoundType.TennisRacket:
                    return tennisRacket[Random.Range(0, tennisRacket.Length)];
                default:
                    return null;
            }
        }
    }
}