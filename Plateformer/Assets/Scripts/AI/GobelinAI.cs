using System;
using Player;
using UnityEngine;

namespace AI
{
    public class GobelinAI : AbstractAI
    {
        [SerializeField] private bool OnlyWhenPlayerNear = true;
        [SerializeField] private float m_distanceChase = 8f;
        [SerializeField] private float m_speed = 3f;
        [SerializeField]
        private LayerMask m_layerMask;
        
        private Rigidbody2D m_rigidbody;
        private SpriteRenderer m_spriteRenderer;
        private bool m_right;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void DoIdle()
        {
            base.DoIdle();
            
            if (!OnlyWhenPlayerNear || Vector2.Distance(PlayerController.instance.transform.position, transform.position) < m_distanceChase)
            {
                ChangeState(States.Wander);
            }
        }

        protected override void DoAttack()
        {
            base.DoAttack();
            if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) >= 1f)
            {
                DoIdle();
            }
        }

        protected override void DoWander()
        {
            base.DoWander();

            if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) < 1f)
            {
                ChangeState(States.Attack);
                return;
            }
            
            m_rigidbody.velocity = new Vector2(m_right ? m_speed : -m_speed, m_rigidbody.velocity.y);

            RaycastHit2D rayFloor = Physics2D.Raycast(transform.position + new Vector3( m_right ? 1:-1, 0 ,0), Vector2.down, 1f, m_layerMask);

            if (!rayFloor)
            {
                m_right = !m_right;
                m_spriteRenderer.flipX = !m_right;
                return;
            }
            
            RaycastHit2D ray = Physics2D.Raycast(transform.position + Vector3.up *0.5f, m_right ? Vector2.right : Vector2.left, 1.5f, m_layerMask);
            if (ray.collider != null)
            {
                m_right = !m_right;
                m_spriteRenderer.flipX = !m_right;
            }
        }
        
    }
}