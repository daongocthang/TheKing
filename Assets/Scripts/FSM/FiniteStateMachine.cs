using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSM
{
    public class FiniteStateMachine
    {
        public EntityState currentState { get; private set; }
        private readonly List<EntityState> states = new List<EntityState>();

        public void Start()
        {
            currentState = states.FirstOrDefault();
            if (currentState == null)
            {
                Debug.LogError($"{this.GetType().Name} not found any state");
                return;
            }

            currentState.Enter();
        }

        public void AddState(EntityState state)
        {
            if (!states.Contains(state))
            {
                states.Add(state);
            }
        }

        public void ChangeState<T>() where T : EntityState
        {
            ChangeState(GetState<T>());
        }

        private void ChangeState(EntityState newState)
        {
            if (currentState == newState) return;
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private T GetState<T>() where T : EntityState
        {
            var state = states.OfType<T>().FirstOrDefault();
            if (state == null)
            {
                Debug.LogWarning($"{typeof(T)} not found on {this.GetType().Name}");
            }

            return state;
        }
    }
}