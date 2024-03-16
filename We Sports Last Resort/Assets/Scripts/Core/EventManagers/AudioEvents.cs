using System;

namespace Core.EventManagers
{
    public class AudioEvents
    {
        #region Player

        public Action OnPlayer_Footstep;

        #endregion

        #region Weapon

        public Action OnWeapon_Block;
        public Action<HitStrength> OnWeapon_Hit;

        #endregion
        
        #region Zombie
        
        public Action<int> OnZombie_Footstep;
        public Action<int> OnZombie_Grunt;
        public Action<int> OnZombie_Hurt;
        public Action<int> OnZombie_Punch;

        #endregion
    }

    public enum HitStrength
    {
        None,
        Weak,
        Medium,
        Strong
    }
}
