using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;

        private float distanceSpeed = 1.5f;
        private bool isRight = true;
        private float timer;
        private float maxTimer;
        private float randomFactor;
        private float randomMovingSpeed;
        private float defaultSpeed;
        private Vector3 velocity;

        private static List<Player> troop = new List<Player>();
        private Rigidbody2D rb;
        private Animator anim;
        private Camera mainCamera;

        private void Awake()
        {
            this.rb = GetComponent<Rigidbody2D>();
            this.anim = GetComponent<Animator>();
            this.mainCamera = Camera.main;
            this.randomFactor = Random.Range(1f, 1.25f);
            this.randomMovingSpeed = Random.Range(0.95f, 1.05f);
            this.maxTimer = Random.Range(0f, 0.3f);
            this.timer = this.maxTimer;
            this.defaultSpeed = this.moveSpeed;

            Player.AddToTroop(this);
        }

        private void FixedUpdate()
        {
            this.rb.velocity = this.velocity * (this.moveSpeed * Mathf.Log10(this.distanceSpeed) * this.randomFactor);
        }

        private void Update()
        {
            HandleMovement();
            
            HandleRotation();
        }

        private void HandleMovement()
        {
            if (Input.GetMouseButton(0))
            {
                var vector = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                this.velocity = (vector - base.transform.position).normalized;
                this.distanceSpeed = Vector3.Distance(vector, base.transform.position);
                if (this.distanceSpeed < 2f)
                {
                    this.distanceSpeed = 2f;
                }

                this.anim.SetBool(AnimState.Move, true);
                this.anim.speed = this.randomMovingSpeed;
                //Debug.DrawLine(base.transform.position,vector,Color.cyan);
                Debug.Log("Vector: "+vector);
            }

            if (!Input.GetMouseButtonUp(0)) return;
            
            this.velocity = Vector3.zero;
            this.anim.SetBool(AnimState.Move, false);
            this.anim.speed = 1f;
        }

        private void HandleRotation()
        {
            if (this.velocity.x < 0f && this.isRight)
            {
                Rotate(-180f,false);
            }else if (this.velocity.x > 0f && !this.isRight)
            {
                Rotate(0f,true);
            }
        }

        private void Rotate(float rotation, bool right)
        {
            this.timer -= Time.deltaTime;
            if(this.timer>0)return;
            
            base.transform.eulerAngles=new Vector3(0f,rotation,0f);
            this.isRight = right;
            this.timer = this.maxTimer;
        }
        public static void AddToTroop(Player player)
        {
            Player.troop.Add(player);
        }

        public static void RemoveFromTroop(Player player)
        {
            Player.troop.Remove(player);
        }
    }
}