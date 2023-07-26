using April;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    [CreateAssetMenu(fileName = "Runtime Collection", menuName = "April/Rumtime/SpawnedObjectContainer")]
    public class RuntimeCollection_InteractionObjects : ScriptableObject
    {
        public SerializableDictionary<InteractionObjectType, List<InteractionBase>> SpawnedObjectContainer 
            = new SerializableDictionary<InteractionObjectType, List<InteractionBase>>();
    }
}

