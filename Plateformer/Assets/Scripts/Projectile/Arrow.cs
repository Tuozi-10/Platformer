using System;
using UnityEngine;

namespace Projectile
{
    public class Arrow : MonoBehaviour
    {
        private Rigidbody2D m_rb2D;
        private BoxCollider2D m_boxCollider2D;

        [SerializeField] private Animator m_animator;
        
        private void Awake()
        {
            m_rb2D = GetComponent<Rigidbody2D>();
            m_boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            m_rb2D.bodyType = RigidbodyType2D.Static;
            m_boxCollider2D.enabled = false;
            transform.SetParent(other.transform);
            m_animator.enabled = false;
        }
    }
}