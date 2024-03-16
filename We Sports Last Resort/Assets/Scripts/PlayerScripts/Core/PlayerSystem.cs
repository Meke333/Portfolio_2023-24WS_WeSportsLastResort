using UnityEngine;

namespace PlayerScripts.Core
{
    public abstract class PlayerSystem : MonoBehaviour
    {
        protected PlayerScript PlayerScript;
        protected PlayerEvents PlayerEvents;

        protected virtual void Awake()
        {
            PlayerScript = transform.root.GetComponent<PlayerScript>();
            PlayerEvents = PlayerScript.PlayerEvents;
        }
    }
}
