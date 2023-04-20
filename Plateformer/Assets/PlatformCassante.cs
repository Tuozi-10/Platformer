using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlatformCassante : MonoBehaviour
{

    [SerializeField] private float m_durationLife = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool done;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!done)
        {
            if (other.CompareTag("Player"))
            {
                done = true;
                StartCoroutine(Fall());
            }
        }
        
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(m_durationLife);
        transform.DOLocalMoveY(transform.position.y - 5, 2.5f).OnComplete(()=>Destroy(gameObject));
    }

}
