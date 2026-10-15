using UnityEngine;

namespace TriviaGame.FSM
{
    public interface IState
    {
        /// <summary>
        /// Call after entering a state. 
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// Call before leaving a state.
        /// </summary>
        public abstract void Exit();
    }
}