using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScripts.EnemyStateMachine.Zombies.Scripts
{
    public class ZombieNameDisplay : MonoBehaviour
    {
        private ZombieScript _zombieScript;
        
        #region Parameters

        private string _name;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject nameDisplay;
        [SerializeField] private NameHolder[] names;
        private int _currentIndex = 0;

        [SerializeField] private bool isRandomlySet;
        [SerializeField] private bool shouldNamesRepeat;

        
        private bool _isNameDisplayActive;
        private bool _isFocused;
        
        #endregion

        #region UnityMethods

        private void Awake()
        {
            _zombieScript = transform.GetComponentInParent<ZombieScript>();
            ProcessAction_onZombieDead();
        }

        private void OnEnable()
        {
            _zombieScript.onFocused += ProcessAction_onFocusing;
            _zombieScript.onZombieDead += ProcessAction_onZombieDead;
            _zombieScript.onTransmittingNames += SetName;
        }

        private void OnDisable()
        {
            _zombieScript.onFocused -= ProcessAction_onFocusing;
            _zombieScript.onZombieDead -= ProcessAction_onZombieDead;
            _zombieScript.onTransmittingNames -= SetName;
        }

        #endregion

        #region Methods

        void SetNewRandomName()
        {
            //WIP only repeatable works, Send Information to EnemyManagerScript to get if name is already used

            int rngName = 0;
            int namesAlreadyUsed = 0;
            int lengthNameArray = names.Length;
            
            do 
            {
                rngName = Random.Range(0, names.Length);
                namesAlreadyUsed++;
            } 
            while ((shouldNamesRepeat && names[rngName].isUsedAlready) 
                   || namesAlreadyUsed < lengthNameArray);
            
            _name = names[rngName].name;
            nameText.text = _name;
        }

        void SetIncrementalNewName()
        {
            //WIP
            //SetIncrementals in EnemyManagerScript
            //GetIncrementals from ZombieScript then

            _name = names[_currentIndex].name;
            nameText.text = _name;
            
            
            
            _currentIndex = (_currentIndex + 1) % names.Length;
            Debug.Log("Name Length: " + ((_currentIndex + 1) % names.Length));
        }

        void SetName(string newName)
        {
           _name = newName;
           nameText.text = _name;
        }
        

        #endregion

        #region EventMethods

        void ProcessAction_onFocusing()
        {
            if (_isFocused)
                return;
            
            /*
            if (isRandomlySet)
                SetNewRandomName();
            else
                SetIncrementalNewName();
              */
            
            nameDisplay.SetActive(true);

            _isFocused = true;
        }
        
        void ProcessAction_onZombieDead()
        {
            nameDisplay.SetActive(false);
            
            _isFocused = false;
        }

        #endregion
    }

    [Serializable]
    public struct NameHolder
    {
        public string name;
        public bool isUsedAlready;
    }
}
