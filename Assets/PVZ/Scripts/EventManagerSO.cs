using System;
using System.Collections.Generic;
using UnityEngine;

namespace PVZ
{
    [CreateAssetMenu(fileName = "New Event Manager", menuName = "PVZ/Event Manager")]
    public class EventManagerSO : ScriptableObject
    {
        private Dictionary<string, Action> _actions
            = new Dictionary<string, Action>();
        private Dictionary<string, Action<int>> _intActions
            = new Dictionary<string, Action<int>>();
        private Dictionary<string, Action<string>> _stringActions
            = new Dictionary<string, Action<string>>();
        private Dictionary<string, Action<string, Sprite, int, float>> _seedPacketActions
            = new Dictionary<string, Action<string, Sprite, int, float>>();

        public void On(string eventName, Action action)
        {
            if (_actions.ContainsKey(eventName)) _actions[eventName] += action;
            else _actions.Add(eventName, action);
        }

        public void On(string eventName, Action<int> action)
        {
            if (_intActions.ContainsKey(eventName)) _intActions[eventName] += action;
            else _intActions.Add(eventName, action);
        }

        public void On(string eventName, Action<string> action)
        {
            if (_stringActions.ContainsKey(eventName)) _stringActions[eventName] += action;
            else _stringActions.Add(eventName, action);
        }

        public void On(string eventName, Action<string, Sprite, int, float> action)
        {
            if (_seedPacketActions.ContainsKey(eventName)) _seedPacketActions[eventName] += action;
            else _seedPacketActions.Add(eventName, action);
        }

        public void Off(string eventName, Action action)
        {
            if (!_actions.ContainsKey(eventName)) return;
            _actions[eventName] -= action;
            if (_actions[eventName] == null) _actions.Remove(eventName);
        }

        public void Off(string eventName, Action<int> action)
        {
            if (!_intActions.ContainsKey(eventName)) return;
            _intActions[eventName] -= action;
            if (_intActions[eventName] == null) _intActions.Remove(eventName);
        }

        public void Off(string eventName, Action<string> action)
        {
            if (!_stringActions.ContainsKey(eventName)) return;
            _stringActions[eventName] -= action;
            if (_stringActions[eventName] == null) _stringActions.Remove(eventName);
        }

        public void Off(string eventName, Action<string, Sprite, int, float> action)
        {
            if (!_seedPacketActions.ContainsKey(eventName)) return;
            _seedPacketActions[eventName] -= action;
            if (_seedPacketActions[eventName] == null) _seedPacketActions.Remove(eventName);
        }

        public void Emit(string eventName)
        {
            if (!_actions.ContainsKey(eventName)) return;
            _actions[eventName]?.Invoke();
        }

        public void Emit(string eventName, int data)
        {
            if (!_intActions.ContainsKey(eventName)) return;
            _intActions[eventName]?.Invoke(data);
        }

        public void Emit(string eventName, string data)
        {
            if (!_stringActions.ContainsKey(eventName)) return;
            _stringActions[eventName]?.Invoke(data);
        }

        public void Emit(string eventName, string name, Sprite icon, int cost, float cooldown)
        {
            if (!_seedPacketActions.ContainsKey(eventName)) return;
            _seedPacketActions[eventName]?.Invoke(name, icon, cost, cooldown);
        }
    }
}
