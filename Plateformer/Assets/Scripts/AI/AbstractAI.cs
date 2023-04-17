using System;
using Data;
using UnityEngine;

namespace AI
{
    public abstract class AbstractAI : MonoBehaviour
    {
        [SerializeField] private EntityScriptable m_dataEntity;
        [SerializeField] protected Animator m_animator;

        public EntityScriptable.EntityData EntityData;

        public enum States
        {
            Wander, 
            Dead,
            Idle, 
            Attack,
            Follow
        }

        private States m_currentState = States.Idle;
        
        private void Awake()
        {
            EntityData = m_dataEntity.GetData();
        }

        private void Update()
        {
            StateMachine();
        }

        private void StateMachine()
        {
            switch (m_currentState)
            {
                case States.Wander: DoWander(); break;
                case States.Dead: DoDead(); break;
                case States.Idle: DoIdle(); break;
                case States.Attack: DoAttack(); break;
                case States.Follow: DoFollow(); break; 
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void DoWander() { }

        protected virtual void DoDead()
        {
            m_animator.Play("Die");
        }
        
        protected virtual void DoIdle() 
        { 
            m_animator.Play("Idle");
        }
        
        protected virtual void DoAttack() { }
        protected virtual void DoFollow() { }

        public void OnHit(int damages)
        {
            EntityData.CurrentData.hp = Mathf.Max(0, EntityData.CurrentData.hp - damages);
            
            if (EntityData.CurrentData.hp == 0)
            {
                ChangeState(States.Dead);
            }
            else
            {
                
                m_animator.Play("hit");
            }
        }
        
        public void ChangeState(States newState)
        {
            m_currentState = newState;
        }
        
    }
}