using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    [CreateAssetMenu(fileName = "New Stove Data", menuName = "April/Create Stove Data")]
    public class StoveData : ScriptableObject
    {
        public float BurningPower;


        public void Initialize()
        {
            BurningPower = 5f;
        }

        public void OnValidate()
        {
            Initialize();
        }
    }
}

