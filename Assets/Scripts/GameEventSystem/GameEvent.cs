using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem {
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
    public class GameEvent : ScriptableObject {
        public string sentString;
        public int sentInt;
        public float sentFloat;
        public bool sentBool;
        public MonoBehaviour sentMonoBehaviour;
        public ScriptableObject sentScriptableObject;

        private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

        [ContextMenu("Raise Event")]
        public void Raise() {
            for (var i = _listeners.Count - 1; i >= 0; i--) {
                _listeners[i].OnEventRaised(this);
            }
        }

        // override Raise to set sentString before raising this
        public void Raise(string sent) {
            sentString = sent;
            Raise();
        }

        // override Raise to set sentInt before raising this
        public void Raise(int sent) {
            sentInt = sent;
            Raise();
        }

        // override Raise to set sentFloat before raising this
        public void Raise(float sent) {
            sentFloat = sent;
            Raise();
        }

        // override Raise to set sentBool before raising this
        public void Raise(bool sent) {
            sentBool = sent;
            Raise();
        }

        // override Raise to set sentMonobehaviour before raising this
        public void Raise(MonoBehaviour sent) {
            sentMonoBehaviour = sent;
            Raise();
        }

        // override Raise to set sentScriptableObject before raising this
        public void Raise(ScriptableObject sent) {
            sentScriptableObject = sent;
            Raise();
        }

        public void RegisterListener(GameEventListener listener) {
            if (!_listeners.Contains(listener)) {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            if (_listeners.Contains(listener)) {
                _listeners.Remove(listener);
            }
        }
    }
}