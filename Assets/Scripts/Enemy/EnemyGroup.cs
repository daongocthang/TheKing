using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyGroup : MonoBehaviour, IAttackingGroup
    {
        private int numOfIdleEnemies;
        private float idleTimer;
        private Vector3 velocity;
        private Vector3 roamingPosition;
        private List<Enemy> childEnemies;
        private EnemyGroupCenter enemyGroupCenter;
        private BoxCollider2D cameraBoxCollider;

        private void Awake()
        {
            this.enemyGroupCenter = base.GetComponent<EnemyGroupCenter>();
            this.cameraBoxCollider = GameObject.Find("Bounds").GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            this.childEnemies = new List<Enemy>(base.GetComponentsInChildren<Enemy>());
            this.enemyGroupCenter.SetTargetsWithEnemies(this.childEnemies.ToArray());
            this.SetRoamingPosition();
        }

        private void Update()
        {
            if (this.idleTimer > 0f)
            {
                this.HandleIdle();
                return;
            }
            this.CalculateMovement();
        }

        private Vector3 RandomRoamingPosition()
        {
            var newPosition = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * Random.Range(5f, 30f);

            return this.enemyGroupCenter.GetCenterPoint() + newPosition;
        }

        private IEnumerator StopDelay(Enemy enemy, float timeToDelay)
        {
            yield return new WaitForSeconds(timeToDelay);
            if (this.idleTimer > 0f && enemy != null)
            {
                enemy.SetIdle();
                this.HandleShrinkingChildEnemiesColliderRadius();
            }

            yield break;
        }

        private void CalculateMovement()
        {
            var bounds = this.enemyGroupCenter.GetGroupBounds();
            var center = bounds.center;

            if (!IsInBounds(bounds)) return;


            foreach (var enemy in this.childEnemies)
            {
                var enemyPosition = enemy.transform.position;
                this.velocity = (this.roamingPosition - enemyPosition).normalized;
                var enemyFromBehind = Vector2.Distance(center, this.roamingPosition) <
                                      Vector2.Distance(enemyPosition, this.roamingPosition);
                
                this.velocity *= enemyFromBehind?1.25f:0.85f ;
                //this.velocity *= 0.85f;
                enemy.SetVelocity(this.velocity);
            }

            if (Vector3.Distance(center, this.roamingPosition) >= 2f) return;

            this.numOfIdleEnemies = 0;
            this.idleTimer = Random.Range(3f, 8f);
            foreach (var enemy2 in this.childEnemies)
            {
                base.StartCoroutine(this.StopDelay(enemy2, Random.Range(0f, 1.25f)));
            }
        }

        private bool IsInBounds(Bounds bounds)
        {
            var center = this.cameraBoxCollider.bounds.center;
            var size = this.cameraBoxCollider.bounds.size;
            var bounds2 = new Bounds(new Vector3(center.x, center.y), new Vector3(size.x, size.y));
            if (bounds2.Intersects(bounds)) return true;
            DestroyEnemyGroup();
            return false;
        }

        private void HandleIdle()
        {
            this.idleTimer -= Time.deltaTime;
            if (this.idleTimer > 0f) return;

            this.SetRoamingPosition();
            foreach (var enemy in this.childEnemies)
            {
                enemy.SetWalking();
            }
        }

        private void HandleShrinkingChildEnemiesColliderRadius()
        {
            this.numOfIdleEnemies++;
            if(this.numOfIdleEnemies!=this.childEnemies.Count) return;
            foreach (var enemy in this.childEnemies)
            {
                enemy.SetInnerCircleColliderRadius(0.01f);
            }
        }

        public Vector2 FindNearestEnemy(Vector2 targetPosition)
        {
            throw new System.NotImplementedException();
        }

        public void SetRoamingPosition()
        {
            var groupBounds = this.enemyGroupCenter.GetGroupBounds();
            var center = groupBounds.center;
            Vector3 newPosition;
            const int layerMask = 8;
            var num = 0;
            RaycastHit2D hit;

            do
            {
                newPosition = this.RandomRoamingPosition();
                var direction = newPosition - center;
                hit = Physics2D.Raycast(center, direction, float.PositiveInfinity, layerMask);
                num++;
            } while (num < 10000 && (!hit || hit.collider.gameObject.name != "Bounds"));

            this.roamingPosition = newPosition;
        }

        public Vector3 GetRoamingPosition()
        {
            return this.roamingPosition;
        }

        public void DestroyEnemyGroup()
        {
            Object.Destroy(base.gameObject);
        }
    }
}