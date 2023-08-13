using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;


namespace PlayerState
{
    public abstract class BaseState
    {
        protected List<IStateAdapter> SystemList;
        public abstract void EnterState(StateManager playerState);
        public abstract void UpdateState(StateManager playerState);
        public abstract void ExitState(StateManager playerState);
    }
}