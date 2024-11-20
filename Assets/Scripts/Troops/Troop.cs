using System;
using FSM;
using UnityEngine;

namespace Troops
{
    public abstract class Troop : Entity
    {
        public float rightFacing = 1f;

        public Vector3 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void LookAt(Vector3 point)
        {
            var dir = Mathf.Lerp(-1f, 1f, point.x - position.x);

            if (Math.Abs(rightFacing - dir) > 0.0001f)
            {
                FlipHorizontal();
            }
        }

        public void MoveTo(Vector3 target, float speed)
        {
            position = Vector3.MoveTowards(position, target, Time.deltaTime * speed);
        }

        public float DistanceTo(Transform other)
        {
            var a = new Vector3(position.x, position.y);
            var otherPosition = other.position;
            var b = new Vector3(otherPosition.x, otherPosition.y);
            return Vector3.Distance(a, b);
        }

        public void FlipHorizontal()
        {
            rightFacing *= -1;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}