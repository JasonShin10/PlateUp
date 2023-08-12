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
        //public SerializableDictionary<CustomerTable_InteractSlot, Transform> dishesPoints
        //    = new SerializableDictionary<CustomerTable_InteractSlot, Transform>();
        public bool customerAssigned;
        public Stack<InteractionItem> dishes = new Stack<InteractionItem>();
        public override bool IsAutoInteractable => false;
        // 하나라도 존재하면 트루 아니면 false / 
        public bool IsAllEmptyTableSlot => !tableSlots.Exists(x => x.assignedCustomer != null);
        
        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;


        public List<Customer> customers = new List<Customer>();
        public List<TableSlotData> tableSlots = new List<TableSlotData>();

        //public List<CustomerTable_InteractSlot> chairPos = new List<CustomerTable_InteractSlot>();
       
        public void GroupCheck()
        {
            if (customers.Count == 2)
            {
                customerAssigned = true;
            }
        }

        public void CustomerCheck()
        {
           if(IsAllEmptyTableSlot && CheckTableClean())
            {
                customerAssigned = false;
                IngameCustomerWaitingSystem.Instance.WaitCustomerComeIn();
            }
        }
        public override void Interact(PlayerController player)
        {
            if (dishes.Count == 0)
                return;

            player.item = dishes.Pop();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            CustomerCheck();
        }

        public bool CheckTableClean()
        {
            if(dishes.Count ==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Exit()
        {

        }
    }
}

