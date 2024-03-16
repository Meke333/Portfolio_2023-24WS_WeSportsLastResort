using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EnemyScripts
{
    public class ResetSpawnTutorial : MonoBehaviour
    {
        [SerializeField] private SpawnEnemiesScript[] spawn;
        [SerializeField] private TutorialTrigger[] tutorial;


        private void OnTriggerEnter(Collider other)
        {
            for (int i = 0; i < spawn.Length; i++)
            {
                spawn[i].ResetTrigger();
            }

            for (int i = 0; i < tutorial.Length; i++)
            {
                tutorial[i].ResetTrigger();
            }

        }
    }
}


