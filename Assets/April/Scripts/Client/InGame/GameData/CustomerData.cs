using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace April
{
    [CreateAssetMenu(fileName = "New Customer Data", menuName = "April/Create Customer Data")]
    public class CustomerData : ScriptableObject
    {
        public float PaitenceValue;
        // Start is called before the first frame update
        public void Initialize()
        {
            PaitenceValue = 4;
        }

        public void OnValidate()
        {
            Initialize();
        }
    }
}