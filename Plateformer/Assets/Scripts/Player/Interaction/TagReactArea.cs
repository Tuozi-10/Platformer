using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagReactArea : MonoBehaviour
{
    private BoxCollider2D m_CollisionArea;
    [SerializeField] string tag = $"Player";
    [SerializeField] UnityEvent OnPlayerEnter;
    [SerializeField] UnityEvent OnPlayerExit;

    private void Awake()
    {
        m_CollisionArea = this.GetComponent<BoxCollider2D>();
        m_CollisionArea.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            OnPlayerExit?.Invoke();
        }
    }
}
