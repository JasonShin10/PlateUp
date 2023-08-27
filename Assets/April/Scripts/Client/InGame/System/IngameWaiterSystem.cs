using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameWaiterSystem : MonoBehaviour
    {
        public static IngameWaiterSystem Instance { get; private set; }

        public List<Character_Waitress> SpawnedWaitlessList = new List<Character_Waitress>();
        public List<Customer> waitingOrderCustomerList = new List<Customer>();
        public List<Customer> waitingFoodCustomerList = new List<Customer>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}