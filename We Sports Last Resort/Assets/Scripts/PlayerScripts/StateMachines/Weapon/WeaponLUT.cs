using System.Collections.Generic;

namespace PlayerScripts.StateMachines.Weapon
{
    public static class WeaponLUT
    {
        private static readonly Dictionary<WeaponType, float> WeaponFullAttackTimeDictionary = new Dictionary<WeaponType, float>()
        {
            { WeaponType.Sword,    0.5f},
            { WeaponType.Gun,      0.2f},
        };

        public static float GetWeaponFullAttackTime(WeaponType weaponType)
        {
            return WeaponFullAttackTimeDictionary[weaponType];
        }
    }
}
