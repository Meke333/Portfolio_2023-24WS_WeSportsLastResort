using PlayerScripts.Core;
using UnityEngine;

namespace PlayerScripts.PlayerSystemScripts
{
    public class PlayerData : PlayerSystem
    {
        [Header("ParticlePosition")]
        public Transform sweatParticle;
        public Transform groundParticle;
        
        protected override void Awake()
        {
            base.Awake();
            PlayerScript.PlayerData = this;
        }
    }
}
