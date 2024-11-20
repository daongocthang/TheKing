
using FSM.Interfaces;
using UnityEngine;

namespace FSM
{
    public abstract class Entity : MonoBehaviour, ITriggerable
    {
        public FiniteStateMachine stateMachine { get; private set; }

        public Animator anim { get; private set; }

        protected abstract void OnCreateStates();

        public virtual void Awake()
        {
            stateMachine = new FiniteStateMachine();

            OnCreateStates();
        }

        public virtual void Start()
        {
            anim = GetComponentInChildren<Animator>();
            stateMachine.Start();
        }

        public virtual void Update()
        {
            stateMachine.currentState.LogicUpdate();
        }

        public virtual void FixedUpdate()
        {
            stateMachine.currentState.PhysicsUpdate();
        }

        public virtual void OnTriggered(int resultCode)
        {
            stateMachine.currentState.AnimTrigger(resultCode);
        }
    }
}