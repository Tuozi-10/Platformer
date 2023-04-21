using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        private bool m_isLookingLeft = false;
        [SerializeField] private Animator m_playerAnimator;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int VelocityFall = Animator.StringToHash("VelocityFall");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        [SerializeField] private KeyCode shootingKey;
        [SerializeField] private KeyCode intercationKey;

        [Header("Movement")]
        [SerializeField] private float m_JumpForce = 10;
        [SerializeField] private float m_jumpMaxDuration = 1.5f;
        [SerializeField] private float m_jumpgravityStrength = 1.5f;
        [Space]
        [SerializeField] private float m_accelForce = 5;
        [SerializeField] private float m_deccelMultiplier = 5;
        [SerializeField] private float m_maxSpeed = 5;
        [SerializeField] private float m_aerialFactor = .25f;
        [SerializeField] private float m_maxFallSpeed = 5;
        [Space]
        [SerializeField] private float groundDist = 1.5f;
        [SerializeField] private LayerMask groundMask;
        private SpriteRenderer m_spriteRenderer;
        private Rigidbody2D m_rigidbody;
        private bool m_canJump = true;
        private bool m_shooting;

        [HideInInspector] public UnityEvent onInteract;

        [Header("Shoot")]
        [SerializeField] private GameObject m_arrow;
        [Space]
        [SerializeField] private float m_arrowMinStrength = 15;
        [SerializeField] private float m_arrowMaxStrength = 40;
        [SerializeField] private float m_fullLoadShootDuration = 0.6f;
        [SerializeField] private Vector3 m_arrowSpawnPos = new Vector3(0.45f, 0.25f, 0f);


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
            FloorCheck();
        }

        private void ManageInputs()
        {
            //SHOOT
            if (Input.GetKeyDown(shootingKey)) Shoot();
            else if (Input.GetKeyUp(shootingKey) && m_shooting) ShootArrow();

            //INTERCT
            if (Input.GetKeyDown(intercationKey))
            {
                onInteract?.Invoke();
            }

            //JUMP
            if (Input.GetKey(KeyCode.Space))
            {
                StartJump();
            }

            if (Input.GetKeyUp(KeyCode.Space) && m_jumping)
            {
                m_rigidbody.gravityScale = m_jumpgravityStrength;
                m_jumping = false;
            }
            else  if (Time.time - m_startJumpLoad > m_jumpMaxDuration && !m_canJump)
            {
                m_rigidbody.gravityScale = m_jumpgravityStrength;
                m_jumping = false;
            }

            //MOVE
            if (Input.GetKey(KeyCode.RightArrow))
            {
                m_spriteRenderer.flipX = m_isLookingLeft = false;
                Move(m_accelForce);
                m_playerAnimator.SetBool(Moving, m_canJump);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_spriteRenderer.flipX = m_isLookingLeft = true;
                Move(-m_accelForce);
                m_playerAnimator.SetBool(Moving, m_canJump);
            }
            else
            {
                float decel = -m_rigidbody.velocity.x * m_deccelMultiplier;
                m_rigidbody.AddForce(Vector2.right * decel * Time.deltaTime, ForceMode2D.Impulse);

                m_playerAnimator.SetBool(Moving, false);
            }

            //Clamp Move
            m_rigidbody.velocity = new Vector2(
                Mathf.Clamp(m_rigidbody.velocity.x, -m_maxSpeed, m_maxSpeed),
                Mathf.Clamp(m_rigidbody.velocity.y, -m_maxFallSpeed, 42)
                );
            m_playerAnimator.SetFloat(VelocityFall, m_rigidbody.velocity.y);
        }

        #region shoot

        private float m_timeStartShoot;
        private void Shoot()
        {
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
            var pos = new Vector2(transform.position.x + ((m_isLookingLeft ? -1 : 1) * m_arrowSpawnPos.x), transform.position.y + m_arrowSpawnPos.y);
            var arrow = Instantiate(m_arrow, pos, Quaternion.identity, null);
            arrow.GetComponent<Rigidbody2D>().AddForce(!m_isLookingLeft ? Vector2.right * strength : -Vector2.right * strength, ForceMode2D.Impulse);
            arrow.GetComponent<Rigidbody2D>().AddForce(m_rigidbody.velocity, ForceMode2D.Impulse);
        }

        #endregion

        private void Move(float speed)
        {
            if (m_shooting) return;
            m_rigidbody.AddForce(Vector2.right * speed * Time.deltaTime * (m_canJump ? 1f : m_aerialFactor), ForceMode2D.Impulse);
        }

        private float m_startJumpLoad;
        private bool m_jumping;
        private void StartJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_canJump)
            {
                m_rigidbody.AddForce(new Vector2(0, m_JumpForce), ForceMode2D.Impulse);
                m_startJumpLoad = Time.time;
                m_rigidbody.gravityScale = 1f;
                m_jumping = true;
                m_canJump = false;
            }
            if (Input.GetKeyUp(KeyCode.Space) && !m_jumping)
            {
                m_rigidbody.gravityScale = 2f;
            }
        }

        private void FloorCheck()
        {
            //if (m_rigidbody.IsTouchingLayers(groundMask))
            if (Physics2D.BoxCast(m_rigidbody.position, m_rigidbody.GetComponent<BoxCollider2D>().size, 0f, Vector2.down, groundDist, groundMask) && !m_jumping)
            {
                Land();
                Debug.DrawRay(m_rigidbody.position, Vector2.down * groundDist, Color.green);
            }
            else
            {
                Debug.DrawRay(m_rigidbody.position, Vector2.down * groundDist, Color.red);
            }

        }

        //Landing
        private void Land()
        {
            if (m_canJump) return;

            m_jumping = false;
            m_canJump = true;
            m_rigidbody.gravityScale = 1f;
            m_playerAnimator.SetBool(Jumping, false);
        }
    }
}