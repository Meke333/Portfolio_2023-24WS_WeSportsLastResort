

namespace PlayerScripts.StateMachines.Weapon.States
{
    public class None : WeaponStateMachine
    {
        
        
        public None(WeaponStateVariableContainer wsvc) : base(wsvc)
        {
            stateName = "None";
            weaponType = WeaponType.None;


        }
    }
}
