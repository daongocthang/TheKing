using System;
using Interfaces;
using Players;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float maxAttackTimer=1f;

        private float attackTimer;
        private bool isPlayer;
        private bool isRightOnStart;
        private GameObject target;
        private Animator anim;
        private IRotation rotation;

        private void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.isPlayer = GetComponent<Player>() != null;
            this.rotation = GetComponent<IRotation>();
        }

        private void Update()
        {
            this.HandleAttack();   
        }

        private void HandleAttack()
        {
            this.attackTimer -= Time.deltaTime;
            if (!this.target) return;
            this.HandleRotation();
            if(this.attackTimer>0f) return;
            this.anim.SetTrigger(AnimState.Attack);
            this.attackTimer = this.maxAttackTimer;
            this.isRightOnStart = this.rotation.IsRight();
        }

        private void HandleRotation()
        {
            var right=this.target.transform.position.x>=base.transform.position.x;
            this.rotation.SetRightFacing(right);
            var rotationY = right ? 0f : -180f;
            base.transform.eulerAngles=new Vector3(0f, rotationY, 0f);
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }
    }
}