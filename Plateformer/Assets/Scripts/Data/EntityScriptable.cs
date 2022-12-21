using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName ="Entity data", menuName = "Data/Entity data")]
    public class EntityScriptable : ScriptableObject
    {
       [SerializeField] private EntityDataStruct data;
        
       // be careful with types not copied by value, you could end with shared references and modify the actual scriptable value
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
            public EntityDataStruct CurrentData;
            public EntityDataStruct InitialData { private set; get; }

            public EntityData(EntityScriptable dataScriptable)
            {
                InitialData = CurrentData = dataScriptable.data;
            }
        }
    }



}