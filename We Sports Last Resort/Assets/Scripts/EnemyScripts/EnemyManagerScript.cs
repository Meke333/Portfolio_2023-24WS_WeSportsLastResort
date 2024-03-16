using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using EnemyScripts.EnemyStateMachine.Zombies;
using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using General.SingletonClass;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScripts
{
    public class EnemyManagerScript : SingletonClass<EnemyManagerScript>
    {
        #region Variables

        public EnemyType enemyType;

        [SerializeField] private int enemyCount;

        [Header("Zombie")] 
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private Queue<ZombieScript> _zombies = new Queue<ZombieScript>();
        [SerializeField] private Queue<ZombieScript> _deadZombies = new Queue<ZombieScript>();
        
        //private Queue<SharkScript> _sharks = new Queue<SharkScript>();

        public RespawnVariables TestingRespawnVariables;

        [SerializeField] private NameHolder[] names;
        //[SerializeField] private bool isNameRandomized;
        private int _namesUsed;

        #endregion

        #region Actions

        public event Action onAllZombiesAreDead;

        #endregion

        #region UnityMethods

        private async void Start()
        {
            await Task.Delay(100);
            //int childCount = gameObject.transform.childCount;

            for (int i = 0; i < enemyCount; i++)
            {
                switch (enemyType)
                {
                    case EnemyType.None:
                        break;
                    case EnemyType.Zombie:
                        
                        //ZombieScript a = gameObject.transform.GetChild(i).GetComponent<ZombieScript>();
                        
                        //if (a != null)
                        //{

                        GameObject temp = Instantiate(zombiePrefab, gameObject.transform);
                        await Task.Yield();
                        ZombieScript a = temp.GetComponent<ZombieScript>();
                        await Task.Yield();
                        //Debug.Log("Zombie Enqueued (" + i + ")");

                        a.name = "Zombie: " + i;
                        a.ZombieNumber = i;
                        _deadZombies.Enqueue(a);
                            
                        //zombies inactive
                        _deadZombies.Peek().SetActive(false);

                        
                        //temp.SetActive(false);
                        //}
                        await Task.Yield();
                        
                        temp.SetActive(false);
                        
                        break;
                    case EnemyType.Shark:
                        /*SharkScript b = gameObject.transform.GetChild(i).GetComponent<SharkScript>();
                        if (b != null)
                        {
                            Debug.Log("Shark Enqueued (" + i ")");
                            _sharks.Enqueue(b);
                        */
                        break;
                    case EnemyType.Alien:
                        break;
                }
                

            }
            /*
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Zombie:
                    //First One in Line is focused
                    _zombies.Peek().SetThisOneZombieAsFocus();
                    break;
                case EnemyType.Shark:
                    break;
                case EnemyType.Alien:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            */
            
            //Testing
            //RespawnEnemy(TestingRespawnVariables);
        }

        private void OnEnable()
        {
            CoreEventManager.Instance.GameEvents.OnPlayerTakingAStep += RecheckIfZombieIsFocused;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnPlayerTakingAStep -= RecheckIfZombieIsFocused;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                RespawnEnemy(TestingRespawnVariables);
            }
        }

        #endregion

        #region Methods

        public Transform GetCurrentEnemyTransform()
        {
            Transform result = null;
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Zombie:
                    result = _zombies.Peek()?.gameObject.transform;
                    break;
                case EnemyType.Shark:
                    //result = _sharks.Peek()?.gameObject.transform;
                    break;
                case EnemyType.Alien:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        //Need to think about when it is Active and when not, maybe need a separate Method with Activating Zombie
        
        
        
        public void RespawnEnemy(RespawnVariables respawnVariables)
        {
            //Debug.Log("debug: 1");
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Zombie:
                    
                    //Debug.Log("debug: 2");
                    //Debug.Log("DeadZombies Count: " + _deadZombies.Count);
                    if(_deadZombies.Count <= 0)
                        return;
                    
                    ZombieScript temp = _deadZombies.Dequeue();
                    temp.gameObject.SetActive(true);
                    //Debug.Log("debug: 3");
                    temp.ResetZombieScript(respawnVariables.Transform, respawnVariables.EnemyHealth, respawnVariables.MoveSpeed, respawnVariables.Name, respawnVariables.GuardDirection);

                    //name: everything randomized
                    int rngNames = 0;
                    do
                    {
                        rngNames = Random.Range(0, names.Length);
                    } while (_namesUsed < names.Length && names[rngNames].isUsedAlready);
                    
                    temp.onTransmittingNames?.Invoke(names[rngNames].name);
                    names[rngNames].isUsedAlready = true;
                    _namesUsed++;
                    
                    temp.SetActive(true);
                    //Debug.Log("debug: 4");
                    
                    _zombies.Enqueue(temp);
                    
                    _zombies.Peek().SetThisOneZombieAsFocus(true);

                    break;
                case EnemyType.Shark:
                    break;
                case EnemyType.Alien:
                    break;
            }
        }

        public void RecheckIfZombieIsFocused()
        {
            if(_zombies.Count <= 0)
                return;
            //Debug.Log("zombies Peek! : " + _zombies.Peek().name);
            _zombies.Peek().SetThisOneZombieAsFocus(true);
        }
        
        

        #endregion


        #region EventMethods

        public void ZombieDead(ZombieScript zombie)
        {
            //if Zombie died the put it in deadZombies Queue

            //Debug.Log("ZombieCount: " + _zombies.Count);
            if (_zombies.Count <= 0)
                return;

            //Debug.Log("zombies dead: queue: " + _zombies.Peek().name + "zombie: " + zombie.name );
            
            if (!_zombies.Contains(zombie))
                return;
            
            if (_zombies.Peek() != zombie)
                return;
            
            _zombies.Peek().SetActive(false);

            _deadZombies.Enqueue(_zombies.Dequeue());
            _deadZombies.Peek().SetActive(false);
            _deadZombies.Peek().SetThisOneZombieAsFocus(false);
            
            //Next One in Line is focused
            if (_zombies.Count <= 0)
            {
                onAllZombiesAreDead?.Invoke();
                return;
            }
            
            //Debug.Log("zombies Peek! : " + _zombies.Peek().name);
            _zombies.Peek().SetThisOneZombieAsFocus(true);
            
            
        }

        #endregion

    }

    [System.Serializable]
    public struct RespawnVariables
    {
        public Transform Transform;
        public float EnemyHealth;
        public float MoveSpeed;
        public string Name;
        public ZombieGuardDirection GuardDirection;
    }

    public enum EnemyType
    {
        None,
        Zombie,
        Shark,
        Alien,
    }
}
