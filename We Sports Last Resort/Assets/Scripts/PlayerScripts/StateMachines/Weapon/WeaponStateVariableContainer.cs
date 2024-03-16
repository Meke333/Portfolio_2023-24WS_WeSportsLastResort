using PlayerScripts.Core;
using UnityEngine;

namespace PlayerScripts.StateMachines.Weapon
{
    public class WeaponStateVariableContainer
    {
        public PlayerScript PlayerScript;
        public PlayerEvents PlayerEvents;
        
        //Constructor
        public WeaponStateVariableContainer(PlayerScript playerScript)
        {
            PlayerScript = playerScript;
            PlayerEvents = playerScript.PlayerEvents;
        }

        #region Initializing & ShutDown

        public void Initializing()
        {
            
        }

        public void ShutDown()
        {
            
        }

        #endregion

        #region EventMethods

        

        #endregion
    }
}
