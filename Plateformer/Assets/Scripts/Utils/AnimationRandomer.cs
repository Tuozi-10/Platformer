
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationRandomer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator .Play(0,-1,Random.Range(0,1f));
        animator.speed = Random.Range(0.925f, 1.125f);
    }

}
