using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class EnemyGroupCenter : MonoBehaviour, IGroupCenter
    {
        private readonly List<Transform> targets = new List<Transform>();

        public void SetTargetsWithEnemies(Enemy[] newTargets)
        {
            foreach (var target in newTargets)
            {
                targets.Add(target.transform);
            }
        }


        public Vector3 GetCenterPoint()
        {
            return GetGroupBounds().center;
        }

        public void RemoveTarget(Transform target)
        {
            this.targets.Remove((target));
        }

        public Bounds GetGroupBounds()
        {
            if (targets.Count == 0)
                return new Bounds(base.transform.position, Vector3.zero);
            if (targets.Count == 1)
                return new Bounds(targets[0].position, Vector3.zero);

            var bounds = new Bounds(targets[0].position, Vector3.zero);
            foreach (var target in targets)
            {
                bounds.Encapsulate(target.position);
            }


            return bounds;
        }


    }
}