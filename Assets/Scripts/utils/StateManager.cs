using System;
using System.Collections.Generic;
using PlayerState;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public abstract class StateManager : MonoBehaviour
    {
        protected BaseState CurrentState;
        protected Dictionary<string, UnityAction> EventList = new Dictionary<string, UnityAction>();

        public void SwitchState(BaseState state)
        {
            CurrentState.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this);
        }

        private void Update()
        {
            CurrentState.UpdateState(this);
        }

        public void SubscribeToEvents(string eventName, UnityAction function)
        {
            EventList[eventName] += function;
        }
        
        public void UnSubscribeToEvents(string eventName, UnityAction function)
        {
            EventList[eventName] -= function;
        }
    }
}