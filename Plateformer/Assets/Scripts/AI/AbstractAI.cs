using System;
using Data;
using UnityEngine;

namespace AI
{
    public class AbstractAI : MonoBehaviour
    {
        [SerializeField] private EntityScriptable m_dataEntity;

        public EntityScriptable.EntityData EntityData;
        
        private void Awake()
        {
            EntityData = m_dataEntity.GetData();
            EntityData.Data.hp = 3;
        }
    }
}