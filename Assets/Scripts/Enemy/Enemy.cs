using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        private float normalSpeed;

        private bool isRight=true;
        private bool isIdle;
        private bool isAttacking;

        private float maxTimer;
        private float timer;
        private float startingInnerCircleColliderRadius;

        private Vector3 velocity;

        private Animator anim;
        private Rigidbody2D rb;
        private CircleCollider2D innerCircleCollider;
        private EnemyGroup enemyGroup;

        private void Awake()
        {
            this.enemyGroup = GetComponentInParent<EnemyGroup>();
            this.anim = GetComponent<Animator>();
            this.rb = GetComponent<Rigidbody2D>();
            this.innerCircleCollider = GetComponent<CircleCollider2D>();
            this.startingInnerCircleColliderRadius = this.innerCircleCollider.radius;
            this.maxTimer = Random.Range(0.2f, 0.8f);
            this.timer = this.maxTimer;
            this.normalSpeed = this.moveSpeed;
            this.anim.SetBool(AnimState.Move, true);
        }

        private void FixedUpdate()
        {
            this.rb.MovePosition(base.transform.position + this.moveSpeed * this.velocity * Time.fixedDeltaTime);
            Debug.DrawLine(base.transform.position, base.transform.position + this.velocity);
        }

        private void Update()
        {
            if (this.enemyGroup)
            {
                Rotate(this.enemyGroup.GetRoamingPosition());
                return;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
        }

        private void Rotate(Vector3 direction)
        {
            if (this.isIdle) return;
            if (direction.x < base.transform.position.x && this.isRight)
            {
                SetFlipHorizontal(false);
            }
            else if (direction.x > base.transform.position.x && !this.isRight)
            {
                SetFlipHorizontal(true);
            }
        }

        private void SetFlipHorizontal(bool right)
        {
            this.timer -= this.maxTimer;
            if (this.timer > 0) return;

            this.isRight = right;
            var y = right ? 0f : -180f;
            base.transform.eulerAngles = new Vector3(0f, y, 0f);
        }

        private IEnumerator IdleRotate()
        {
            while (this.isIdle)
            {
                if (Random.Range(1, 101) <= 33)
                {
                    this.SetFlipHorizontal(!isRight);
                }

                yield return new WaitForSecondsRealtime(1.5f);
            }

            yield break;
        }

        public void SetVelocity(Vector3 vector)
        {
            this.velocity = vector;
        }

        public void SetIdle()
        {
            this.velocity = Vector3.zero;
            this.isIdle = true;
            base.StartCoroutine(IdleRotate());
            this.anim.SetBool(AnimState.Move, false);
        }

        public void SetWalking()
        {
            this.isIdle = false;
            this.anim.SetBool(AnimState.Move, true);
            SetInnerCircleColliderRadius(this.startingInnerCircleColliderRadius);
        }

        public void SetInnerCircleColliderRadius(float radius)
        {
            this.innerCircleCollider.radius = radius;
        }
    }
}