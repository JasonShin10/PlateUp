using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerWaitingSystem : MonoBehaviour
    {
        public static IngameCustomerWaitingSystem Instance { get; private set; } = null;

        public bool IsFullWaitingSlot => !waitingSlots.Exists(x => x.customer == null);



        public List<IngameCustomerWaitingSystem_SlotData> waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>();

        private void Awake()
        {
            Instance = this;

            var slots = GetComponentsInChildren<IngameCustomerWaitingSystem_SlotData>();
            waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>(slots);
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}

