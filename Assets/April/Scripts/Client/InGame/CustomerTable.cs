using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerTable : MonoBehaviour
    {
        public SerializableDictionary<CustomerTable_InteractSlot, Transform> dishesPoints 
            = new SerializableDictionary<CustomerTable_InteractSlot, Transform>();


    }
}

