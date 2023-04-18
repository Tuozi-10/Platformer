using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour
{
    private BoxCollider2D m_CollisionArea;

    private void Awake()
    {
        m_CollisionArea = this.GetComponent<BoxCollider2D>();
        m_CollisionArea.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag($"Player"))
        {
            var life = collision.GetComponent<LifeSystem>();
            life.InstantKill();
        }
    }

}
