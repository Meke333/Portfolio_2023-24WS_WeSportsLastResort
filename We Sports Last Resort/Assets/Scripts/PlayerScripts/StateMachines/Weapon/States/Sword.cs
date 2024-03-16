

namespace PlayerScripts.StateMachines.Weapon.States
{
    public class Sword : WeaponStateMachine
    {
        
        
        
        public Sword(WeaponStateVariableContainer wsvc) : base(wsvc)
        {
            stateName = "Sword";
            weaponType = WeaponType.Sword;

            
        }
    }
}
