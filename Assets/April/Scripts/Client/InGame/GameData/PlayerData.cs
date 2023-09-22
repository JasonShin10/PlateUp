using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "April/Create Player Data")]
    public class PlayerData : ScriptableObject
    {
        public float PlayerSpeed;

        public void Initialize()
        {
            PlayerSpeed = 3f;
        }

        public void OnValidate()
        {
            Initialize();
        }
    }
}
