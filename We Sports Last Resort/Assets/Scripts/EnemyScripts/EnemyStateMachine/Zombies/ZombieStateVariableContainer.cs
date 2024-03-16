using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using PlayerScripts.Core;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies
{
    [System.Serializable]
    public class ZombieStateVariableContainer
    {
        public ZombieScript ZombieScript;
        //public ZombieEvents ZombieEvents;

        public bool isActive;

        public bool isFinishedTransmittingData;

        public float Health;
        public float MoveSpeed;
        public bool CanMove = true;
        public float DistanceBetweenPlayerAndEnemy = 2.25f;
        public float FocusingDistanceToPlayer = 10;
        public Vector3 hitDirection;
        
        
        public AttackDirection AttackDirection;


        public Transform zombiePosition;
        public Transform playerPosition;


        //Constructor
        public ZombieStateVariableContainer(ZombieScript zombieScript, float health, float moveSpeed)
        {
            ZombieScript = zombieScript;
            zombiePosition = zombieScript.transform;
            playerPosition = PlayerScript.Instance.playerTransform;

            Health = health;
            MoveSpeed = moveSpeed;
        }
        
        //Initalization & ShutDown
        public void Initialize()
        {
            ZombieScript.onTakingDamage += value => Health -= value;
        }

        public void ShutDown()
        {
            ZombieScript.onTakingDamage -= value => Health -= value;
        }
        
        //Methods
        public float GetDistanceFromEnemyToTarget()
        {
            Vector3 player = playerPosition.position;
            Vector3 zombie = zombiePosition.position;
            player.y = 0;
            zombie.y = 0;
            
            //Debug.LogError("Player Position: " + player + "; Zombie Position: "+ zombie + "; Distance: " + Vector3.Distance(player, zombie));

            
            
            
            return Vector3.Distance(player, zombie);
            
        }

    }
}
