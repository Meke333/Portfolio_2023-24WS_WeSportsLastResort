using Core;
using General.SingletonClass;
using UnityEngine;
using PlayerScripts.StateMachines.Player;
using PlayerScripts.PlayerSystemScripts;
using PlayerScripts.StateMachines.Player.States;
using PlayerScripts.StateMachines.Weapon;
using PlayerScripts.StateMachines.Weapon.States;

namespace PlayerScripts.Core
{
    public class PlayerScript : SingletonClass<PlayerScript>
    {
        #region Parameters

        [SerializeField] private int _hitPoints = 3;
        
        public Transform playerTransform;
        public Transform camera_FollowPosition;
        
        public PlayerEvents PlayerEvents = new PlayerEvents();
        public PlayerData PlayerData;
        private PlayerProcessWiiMote _playerProcessWiiMote;
        private PlayerProcessBalanceBoard _playerProcessBalanceBoard;
        
        #endregion
        
        #region StateMachine

        [SerializeField] private PlayerStateMachine currentPlayerState;
        [SerializeField] private WeaponStateMachine currentWeaponState;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            base.Awake();

            playerTransform = gameObject.transform;
            
            //FILL IN THE PLAYERSTATEVARIABLE CLASS
            currentPlayerState = new Idle(new PlayerStateVariableContainer(this));
            currentWeaponState = new Sword(new WeaponStateVariableContainer(this));
            currentPlayerState.playerStateVariableContainer.Initializing();
            currentWeaponState.weaponStateVariableContainer.Initializing();

            _playerProcessBalanceBoard = new PlayerProcessBalanceBoard(this);
            _playerProcessWiiMote = new PlayerProcessWiiMote(this);
        }

        private void Update()
        {
            currentPlayerState = (PlayerStateMachine) currentPlayerState.Process();
            currentWeaponState = (WeaponStateMachine) currentWeaponState.Process();
        }

        private void OnEnable()
        {
            
            
            _playerProcessWiiMote.Initialize();
            _playerProcessBalanceBoard.Initialize();

        }

        private void OnDisable()
        {
            
            _playerProcessWiiMote.ShutDown();
            _playerProcessBalanceBoard.ShutDown();
            
        }

        #endregion

        #region Methods

        public int GetHealth()
        {
            return _hitPoints;
        }
        
        public void TakeDamage()
        {
            _hitPoints--;
            PlayerEvents.onPlayerStateMachine_TakingDamage?.Invoke(1);
            
            CoreEventManager.Instance.UIEvents.OnPlayerHealth?.Invoke(_hitPoints);

            if (_hitPoints > 0)
                return;
            
            CoreEventManager.Instance.GameEvents.OnPlayerDied?.Invoke();
            CoreEventManager.Instance.GameEvents.OnTransmitPlayerPosition?.Invoke(gameObject.transform);
        }

        #endregion
        
        
    }
}
