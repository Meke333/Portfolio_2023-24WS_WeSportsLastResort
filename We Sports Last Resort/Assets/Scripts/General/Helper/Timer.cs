using System;
using System.Threading.Tasks;
using UnityEngine;

namespace General.Helper
{
    public class Timer
    {
        public float endTime;
        private float _currentTime;

        private bool _isTimerActive;
        private bool _isTimerInterrupted;

        public event Action onTimerDone;

        public Timer(float _endTime)
        {
            endTime = _endTime;
        }

        public async void RunTimer()
        {
            if (_isTimerActive)
                return;
            _isTimerActive = true;

            while (!_isTimerInterrupted && _currentTime < endTime)
            {
                _currentTime += Time.deltaTime;

                await Task.Yield();
            }
            
            _isTimerActive = false;
            
            if (_isTimerInterrupted)
                return;
            
            onTimerDone?.Invoke();
        }

        public async void ResetAndRunTimer()
        {
            _isTimerInterrupted = true;
            _currentTime = 0;

            
            while(_isTimerActive)
            {
                await Task.Yield();
            }

            _isTimerInterrupted = false;

            RunTimer();

        }

        public async void ResetTimer()
        {
            _isTimerInterrupted = true;
            _currentTime = 0;

            
            while(_isTimerActive)
            {
                await Task.Yield();
            }

            _isTimerInterrupted = false;
        }

        public void InterruptTimer()
        {
            _isTimerInterrupted = true;
            _currentTime = 0;
        }

    }
}
