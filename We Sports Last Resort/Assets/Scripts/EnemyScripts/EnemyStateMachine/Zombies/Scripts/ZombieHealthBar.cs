using UnityEngine;
using UnityEngine.UI;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieHealthBar : MonoBehaviour
    {
        private ZombieScript _zombieScript;

        #region Parameter

        private float _healthPercentage;

        [SerializeField] private Slider slider;
        [SerializeField] private GameObject healthBar;

        private float _maxHealth;
        private float _currentHealth;

        private bool _isHealthBarActive;
        private bool _isFocused;

        #endregion

        #region UnityMethod

        private void Awake()
        {
            _zombieScript = transform.GetComponentInParent<ZombieScript>();
            ProcessAction_onZombieDead();
        }

        private void OnEnable()
        {
            _zombieScript.onFocused += ProcessAction_onFocusing;
            _zombieScript.onZombieDead += ProcessAction_onZombieDead;
            _zombieScript.onTakingDamage += SetHealth;
            _zombieScript.onReset += SetMaxHealth;
        }

        private void OnDisable()
        {
            _zombieScript.onFocused -= ProcessAction_onFocusing;
            _zombieScript.onZombieDead -= ProcessAction_onZombieDead;
            _zombieScript.onTakingDamage -= SetHealth;
            _zombieScript.onReset -= SetMaxHealth;
        }

        #endregion

        #region Methods

        void SetMaxHealth()
        {
            _maxHealth = _zombieScript.enemyHealth;
            _currentHealth = _maxHealth;
            ProcessPercentage();
        }

        void SetHealth(float damage)
        {
            _currentHealth = _zombieScript.enemyHealth;
            ProcessPercentage();
        }

        void ProcessPercentage()
        {
            slider.value = Mathf.Clamp((_currentHealth / _maxHealth), 0.05f, 1f);
        }

        #endregion

        #region EventMethods

        void ProcessAction_onFocusing()
        {
            if (_isFocused)
                return;
            
            healthBar.SetActive(true);
            
            _isFocused = true;
        }

        void ProcessAction_onZombieDead()
        {
            healthBar.SetActive(false);
            
            _isFocused = false;
        }

        #endregion
    }
}
