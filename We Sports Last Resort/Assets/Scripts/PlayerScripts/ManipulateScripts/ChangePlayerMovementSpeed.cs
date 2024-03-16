using System;
using PlayerScripts.Core;
using UnityEngine;

namespace PlayerScripts.ManipulateScripts
{
    public class ChangePlayerMovementSpeed : MonoBehaviour
    {
        private bool _isActivated;
        [SerializeField] private LayerMask playerMask;

        [SerializeField] private float playerSpeed;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isActivated)
                return;
            
            if (playerMask != (playerMask | 1 << other.gameObject.layer))
                return;

            _isActivated = true;
            
            PlayerScript.Instance.PlayerEvents.onMovement_ChangeSpeed?.Invoke(playerSpeed);


        }
    }
}
