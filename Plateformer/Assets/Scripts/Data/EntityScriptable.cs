using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName ="Entity data", menuName = "Data/Entity data")]
    public class EntityScriptable : ScriptableObject
    {
       [SerializeField] private EntityDataStruct data;
        
        [Serializable]
        public struct EntityDataStruct
        {
            public int hp;
            public int armor;
            public float speed;
            public int damages;
            public string name;
        }

        public EntityData GetData()
        {
            return new EntityData(this);
        }
        
        public class EntityData
        {
            public EntityDataStruct Data;

            public EntityData(EntityScriptable dataScriptable)
            {
                Data = dataScriptable.data;
            }
        }
    }



}