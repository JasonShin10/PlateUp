using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    [System.Serializable]
    public class TableSlotData
    {
        public bool IsAssigned => assignedCustomer != null;

        public Customer assignedCustomer;
        public Transform position;
    }

    public class CustomerTable : InteractionBase
    {
        public SerializableDictionary<CustomerTable_InteractSlot, Transform> dishesPoints
            = new SerializableDictionary<CustomerTable_InteractSlot, Transform>();
        public bool customerAssigned;
        public InteractionItem item;
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;


        public List<Customer> customers = new List<Customer>();
        public List<TableSlotData> tableSlots = new List<TableSlotData>();

        public List<CustomerTable_InteractSlot> chairPos = new List<CustomerTable_InteractSlot>();

        public void GroupCheck()
        {
            if (customers.Count == 2)
            {
                customerAssigned = true;
            }

        }
        public override void Interact(PlayerController player)
        {
            if (item == null)
                return;

            player.item = item;
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
        }

        public override void Exit()
        {

        }
    }
}

