using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Player;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractionArea : MonoBehaviour
{
    private BoxCollider2D m_CollisionArea;
    [SerializeField] UnityEvent OnInteraction;

    private void Awake()
    {
        m_CollisionArea = this.GetComponent<BoxCollider2D>();
        m_CollisionArea.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag($"Player"))
        {
            var control = collision.GetComponent<PlayerController>();
            control.onInteract.AddListener(Interaction);
        }
    }

    public void Interaction()
    {
        OnInteraction?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag($"Player"))
        {
            var control = collision.GetComponent<PlayerController>();
            control.onInteract.RemoveListener(Interaction);
        }
    }
}
