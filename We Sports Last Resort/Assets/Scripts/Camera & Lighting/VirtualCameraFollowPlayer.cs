using Cinemachine;
using EnemyScripts;
using PlayerScripts.Core;
using UnityEngine;

namespace Camera___Lighting
{
    public class VirtualCameraFollowPlayer : MonoBehaviour
    {
        private CinemachineVirtualCamera _cvc;

        private void Start()
        {
            _cvc = GetComponent<CinemachineVirtualCamera>();
            //Debug.LogWarning("Is CinemachineVirtualCamera null?: " + (_cvc == null));

            //_cvc.LookAt = EnemyManagerScript.Instance.GetCurrentEnemyTransform();
        }
    }
}
