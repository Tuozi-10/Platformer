using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Transform m_toFollow;

        private void Start()
        {
            m_toFollow = PlayerController.instance.transform;
        }

        private void Update()
        {
            transform.DOKill();
            transform.DOMoveX(m_toFollow.position.x, 0.95f);
        }
    }
}
