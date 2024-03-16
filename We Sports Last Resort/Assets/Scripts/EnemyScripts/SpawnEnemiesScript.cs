using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EnemyScripts
{
    public class SpawnEnemiesScript : MonoBehaviour
    {
        
        
        private bool _isActivated;
        [SerializeField] private LayerMask playerMask;
        
        [SerializeField] private RespawnVariables[] enemySpawnPoints;
        [SerializeField] private EnemyType enemyType;
        

        private async void OnTriggerEnter(Collider other)
        {
            if (_isActivated)
                return;
            
            if (playerMask != (playerMask | 1 << other.gameObject.layer))
                return;
            
            _isActivated = true;

            switch (enemyType)
            {
                case EnemyType.None:
                    return;
                case EnemyType.Zombie:
                    for (int i = 0; i < enemySpawnPoints.Length; i++)
                    {
                        
                        //Debug.Log(enemySpawnPoints[i].Transform);
                        EnemyManagerScript.Instance.RespawnEnemy(enemySpawnPoints[i]);

                        await Task.Delay(100);
                    }
                    
                    break;
                case EnemyType.Shark:
                    break;
                case EnemyType.Alien:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void ResetTrigger()
        {
            _isActivated = false;
        }
    }
}