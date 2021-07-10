using System;
using UnityEngine;

namespace PVZ
{
    public class Timer
    {
        private float _maxTime = 0;
        private float _timeRemaining = 0;
        private Action _onDone = null;
        private bool _startOnReset = true;

        public bool IsRunning { get; private set; } = false;
        public float TimePercentage => _timeRemaining / _maxTime;

        public Timer(float maxTime, Action onDone = null, bool startOnReset = true)
        {
            _maxTime = maxTime;
            _timeRemaining = _maxTime;
            _onDone = onDone;
            _startOnReset = startOnReset;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset(float newMaxTime = 0)
        {
            if (!Mathf.Approximately(newMaxTime, 0)) _maxTime = newMaxTime;
            _timeRemaining = _maxTime;
            IsRunning = _startOnReset;
        }

        public void Tick()
        {
            if (!IsRunning) return;
            if (Mathf.Approximately(_timeRemaining, 0))
            {
                Reset();
                _onDone?.Invoke();
            }
            else _timeRemaining = Mathf.Clamp(_timeRemaining - Time.deltaTime, 0, _maxTime);
        }
    }
}
