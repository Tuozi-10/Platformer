﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator m_playerAnimator;
        
        [Header("Attack"), Space, SerializeField] 
        private GameObject m_arrow;

        [Header("Movement"), Space, SerializeField]
        private float m_speedJumpMax = 10;  
        [ SerializeField] private float m_speedJumpMin = 10;  
        [ SerializeField] private float m_jumpBuffer = 0.5f;  
        
        [ SerializeField] private float m_speed = 5;
        [ SerializeField] private float m_maxSpeed = 5;

        private SpriteRenderer m_spriteRenderer;
        private Rigidbody2D m_rigidbody;
        private bool m_canJump = true;
        private bool m_shooting;

        [HideInInspector] public UnityEvent onInteract;

        public static PlayerController instance;
        
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int VelocityFall = Animator.StringToHash("VelocityFall");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        private bool m_isLookingLeft = false;
        private Vector3 m_arrowSpawn = new Vector3(0.45f, 0.25f, 0f);

        [SerializeField] private float m_fullLoadShootDuration = 0.6f;
        [SerializeField] private float m_arrowMinStrength = 15;
        [SerializeField] private float m_arrowMaxStrength = 40;

        [SerializeField] private KeyCode shootingKey;
        [SerializeField] private KeyCode intercationKey;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }

            instance = this;
            Init();
        }

        private void Init()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_spriteRenderer = m_playerAnimator.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            ManageInputs();
        }

        private void ManageInputs()
        {
            if (Input.GetKeyDown(shootingKey)) Shoot();
            else if(Input.GetKeyUp(shootingKey) && m_shooting) ShootArrow();

            if (Input.GetKeyDown(intercationKey))
            {
                onInteract?.Invoke();
            }


            if (Input.GetKey(KeyCode.Space))
            {
                StartJump();
            }
            else
            {
                m_jumping = false;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                m_spriteRenderer.flipX = m_isLookingLeft=  false;
                Move(m_speed);
                m_playerAnimator.SetBool(Moving, m_canJump);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_spriteRenderer.flipX = m_isLookingLeft= true;
                Move(-m_speed);
                m_playerAnimator.SetBool(Moving, m_canJump);
            }
            else
            {
                m_playerAnimator.SetBool(Moving, false);
            }
                
            m_rigidbody.velocity = new Vector2(Mathf.Clamp(m_rigidbody.velocity.x, -m_maxSpeed, m_maxSpeed),
                Mathf.Clamp(m_rigidbody.velocity.y, -8, 8));
            
            m_playerAnimator.SetFloat(VelocityFall, m_rigidbody.velocity.y);
        }

        #region shoot

        private float m_timeStartShoot;
        
        private void Shoot()
        {
            if (!m_canJump) return;
            m_timeStartShoot = Time.time;
            m_shooting = true;
            m_playerAnimator.SetBool(Attacking, true);
        }

        private void ShootArrow()
        {
            // calculate loading %
            float ratioShoot = (Time.time - m_timeStartShoot) / m_fullLoadShootDuration;
            
            // calculate min strength + ratio loading
            float strength = m_arrowMinStrength + (m_arrowMaxStrength - m_arrowMinStrength) * ratioShoot;
            
            m_shooting = false;
            m_playerAnimator.SetBool(Attacking, false);
            var pos = new Vector2(transform.position.x + ((m_isLookingLeft ? -1 : 1) * m_arrowSpawn.x), transform.position.y + m_arrowSpawn.y);
            var arrow = Instantiate(m_arrow,  pos, Quaternion.identity, null );
            arrow.GetComponent<Rigidbody2D>().AddForce(!m_isLookingLeft ? Vector2.right *strength : -Vector2.right *strength,ForceMode2D.Impulse );
        }
        
        #endregion
        
        private void Move(float speed)
        {
            if (m_shooting) return;
            m_rigidbody.AddForce(Vector2.right * speed * Time.deltaTime *( m_canJump ? 1f:0.25f), ForceMode2D.Impulse);
        }

        private float m_startJumpLoad;
        private bool m_jumping;
        
        private void StartJump()
        {
            if (m_shooting) return;
            
            if (Input.GetKeyDown(KeyCode.Space) && m_canJump)
            {
                m_rigidbody.AddForce(new Vector2(0, m_speedJumpMin), ForceMode2D.Impulse);
                m_startJumpLoad = Time.time;
                m_jumping = true;
                m_canJump = false;
            }
            
            if (Time.time - m_startJumpLoad < m_jumpBuffer && m_jumping)
            {        
                m_playerAnimator.SetBool(Jumping, true);
                m_rigidbody.AddForce(new Vector2(0, m_speedJumpMax), ForceMode2D.Impulse);
            }
            else
            {
                m_jumping = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag($"NoJump"))
                return;
            
            m_jumping = false;
            m_canJump = true;
            m_playerAnimator.SetBool(Jumping, false);
        }
    }
}