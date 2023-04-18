using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//à attacher sur le player
[ExecuteAlways]
public class LifeSystem : MonoBehaviour
{
    [SerializeField, Range(1, 10)] int maxLifePoint = 3;
    [SerializeField, Range(0f, 1f)] float invulnerabilityDuration = 12/60;
    [SerializeField] List<GameObject> LifePointObjects = new List<GameObject>(3);
    public int lifePoint { get; private set; }
    public bool isDead { get; private set; }
    public UnityEvent onDeath;


    private float invulnerabilityLastHitTime;
    private Vector3 respawnPos;

    private void Awake()
    {
        respawnPos = transform.position;
        Revive();
    }
    private void Update()
    {
        while(LifePointObjects.Count > maxLifePoint)
        {
            LifePointObjects.RemoveAt(LifePointObjects.Count-1);
        }
        while(LifePointObjects.Count < maxLifePoint)
        {
            LifePointObjects.Add(null);
        }

        if (!Application.isPlaying) return;
            
        for (int i = 0; i < LifePointObjects.Count; i++)
        {
            if (LifePointObjects[i] == null) continue;
            LifePointObjects[i].SetActive(i <= lifePoint);
        }
    }


    public void Revive()
    {
        isDead = false;
        lifePoint = maxLifePoint;
        transform.position = respawnPos;
    }
    public void SetCheckPoint(Vector3 pos)
    {
        respawnPos = pos;
    }

    public void InstantKill() => TakeDamage(maxLifePoint);
    public void TakeDamage() => TakeDamage(1);
    public void TakeDamage(int value)
    {
        if (Time.time - invulnerabilityLastHitTime < invulnerabilityDuration) return;
        invulnerabilityLastHitTime = Time.time;

        lifePoint = Mathf.Min(0, lifePoint - 1);
        if (!isDead && lifePoint == 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }

}
