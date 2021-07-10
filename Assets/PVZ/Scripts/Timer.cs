using System;
using UnityEngine;

namespace PVZ
{
    public class Timer
    {
        private float _maxTime = 0;
        private float _timeRemaining = 0;
        private bool _isRunning = false;
        private Action _onDone = null;
        private bool _startOnReset = true;

        public Timer(float maxTime, Action onDone, bool startOnReset = true)
        {
            _maxTime = maxTime;
            _timeRemaining = _maxTime;
            _onDone = onDone;
            _startOnReset = startOnReset;
        }

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Reset(float newMaxTime = 0)
        {
            if (!Mathf.Approximately(newMaxTime, 0)) _maxTime = newMaxTime;
            _timeRemaining = _maxTime;
            _isRunning = _startOnReset;
        }

        public void Tick()
        {
            if (!_isRunning) return;
            if (Mathf.Approximately(_timeRemaining, 0))
            {
                _onDone?.Invoke();
                Reset();
            }
            else _timeRemaining = Mathf.Clamp(_timeRemaining - Time.deltaTime, 0, _maxTime);
        }
    }
}
