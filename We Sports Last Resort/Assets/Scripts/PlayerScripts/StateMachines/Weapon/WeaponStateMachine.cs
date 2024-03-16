using General.StateBase;

namespace PlayerScripts.StateMachines.Weapon
{
    [System.Serializable]
    public abstract class WeaponStateMachine : StateBase
    {
        protected WeaponType weaponType;

        public WeaponStateVariableContainer weaponStateVariableContainer { get; protected set; }

        public WeaponStateMachine(WeaponStateVariableContainer wsvc)
        {
            weaponStateVariableContainer = wsvc;
        }

        public override void Enter()
        {
            base.Enter();

            #region Events

            
            
            #endregion
            
            weaponStateVariableContainer.PlayerEvents.onWeaponStateMachine_TransmitNewWeaponType?.Invoke(weaponType);

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
    }
}
