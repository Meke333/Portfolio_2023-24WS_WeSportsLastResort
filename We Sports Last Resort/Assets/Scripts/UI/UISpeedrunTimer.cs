using System;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UISpeedrunTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI minutes;
        [SerializeField] private TextMeshProUGUI seconds;
        [SerializeField] private TextMeshProUGUI milliseconds;
        
        [SerializeField] private TextMeshProUGUI symbol1;
        [SerializeField] private TextMeshProUGUI symbol2;

        [SerializeField] private Color successColor;

        private DateTime _startTime;
        private TimeSpan _timeElapsed;
        
        private bool _isActive;

        private async void OnEnable()
        {
            
            
            await Task.Yield();
            
            CoreEventManager.Instance.GameEvents.OnPlayerDied += ProcessAction_OnPlayerDied;
            CoreEventManager.Instance.GameEvents.OnLevel1Finished += ProcessAction_OnLevel1Finished;
            CoreEventManager.Instance.GameEvents.OnLevelStarted += ProcessAction_OnLevelStarted;
            CoreEventManager.Instance.GameEvents.OnStartMenu += ProcessAction_OnStartMenu;
        }

        private void OnDisable()
        {
            CoreEventManager.Instance.GameEvents.OnPlayerDied -= ProcessAction_OnPlayerDied;
            CoreEventManager.Instance.GameEvents.OnLevel1Finished -= ProcessAction_OnLevel1Finished;
            CoreEventManager.Instance.GameEvents.OnLevelStarted -= ProcessAction_OnLevelStarted;
            CoreEventManager.Instance.GameEvents.OnStartMenu -= ProcessAction_OnStartMenu;
        }

        private void Update()
        {
            if (!_isActive)
                return;

            _timeElapsed = DateTime.Now - _startTime;

            minutes.text = $"{_timeElapsed.Minutes:00}";
            seconds.text = $"{_timeElapsed.Seconds:00}";
            milliseconds.text = $"{_timeElapsed.Milliseconds:000}";
    }

        void StartTimer()
        {
            Debug.Log("START TIMER");
            
            _startTime = DateTime.Now;
            _isActive = true;
        }

        void StopTimer()
        {
            Debug.Log("STOP TIMER");
            
            _isActive = false;
        }

        void ResetTimer()
        {
            
            Debug.Log("RESET TIMER");
            
            _startTime = DateTime.Now;
            _timeElapsed = new TimeSpan(0, 0, 0, 0,0);
            
            minutes.text = $"{_timeElapsed.Minutes:00}";
            seconds.text = $"{_timeElapsed.Seconds:00}";
            milliseconds.text = $"{_timeElapsed.Milliseconds:000}";
        }

        void ChangeTextColor(Color color)
        {
            
            Debug.Log("COLOR TIMER");
            
            minutes.color = color;
            seconds.color = color;
            milliseconds.color = color;
            symbol1.color = color;
            symbol2.color = color;
        }

        #region EventMethods

        void ProcessAction_OnPlayerDied()
        {
            ChangeTextColor(Color.red);
            StopTimer();
        }

        void ProcessAction_OnLevel1Finished()
        {
            ChangeTextColor(Color.yellow);
            StopTimer();
        }

        void ProcessAction_OnLevelStarted()
        {
            ChangeTextColor(Color.white);
            ResetTimer();
            StartTimer();
        }

        void ProcessAction_OnStartMenu()
        {
            StopTimer();
            ChangeTextColor(Color.white);
            ResetTimer();
        }


        #endregion
    }
}
