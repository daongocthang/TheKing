using System;
using UnityEngine;

namespace FSM
{
    public class EntityState
    {
        protected readonly FiniteStateMachine stateMachine;
        protected readonly Entity entity;
        protected readonly string animBoolName;
        protected float startTime;

        public EntityState(Entity entity, string animBoolName)
        {
            this.entity = entity;
            this.animBoolName = animBoolName;

            this.stateMachine = entity.stateMachine;
            stateMachine.AddState(this);
        }

        public virtual void Enter()
        {
            entity.anim?.SetBool(animBoolName, true);
            startTime = Time.time;
            DoChecks();
        }

        public virtual void Exit()
        {
            entity.anim?.SetBool(animBoolName, false);
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks()
        {
        }

        public virtual void AnimTrigger(int resultCode)
        {
        }
    }
}