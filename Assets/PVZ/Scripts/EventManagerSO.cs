using System;
using System.Collections.Generic;
using UnityEngine;

namespace PVZ
{
    [CreateAssetMenu(fileName = "New Event Manager", menuName = "PVZ/Event Manager")]
    public class EventManagerSO : ScriptableObject
    {
        private Dictionary<string, Action<int>> _intActions = new Dictionary<string, Action<int>>();

        public void On(string eventName, Action<int> action)
        {
            if (_intActions.ContainsKey(eventName)) _intActions[eventName] += action;
            else _intActions.Add(eventName, action);
        }

        public void Off(string eventName, Action<int> action)
        {
            if (!_intActions.ContainsKey(eventName)) return;
            _intActions[eventName] -= action;
            if (_intActions[eventName] == null) _intActions.Remove(eventName);
        }

        public void Emit(string eventName, int data)
        {
            if (!_intActions.ContainsKey(eventName)) return;
            _intActions[eventName]?.Invoke(data);
        }
    }
}
