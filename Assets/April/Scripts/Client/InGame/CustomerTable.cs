using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace April
{
    [System.Serializable]
    public class TableSlotData
    {
        public bool IsAssigned => assignedCustomer != null;

        public Customer assignedCustomer;
        public Customer arrivedCustomer;
        public Transform seatTransform;
        public Transform foodTransform;
    }

    public class CustomerTable : InteractionBase
    {
        public bool IsEmptyTable => !hasCustomerAssigned;
        public bool isAlone = false;
        public bool hasCustomerAssigned;
        public Stack<InteractionItem> dishes = new Stack<InteractionItem>();
        public override bool IsAutoInteractable => false;

        public bool IsAllCustomerArrived => !tableSlots.Exists(x => x.arrivedCustomer == null);
        public bool IsAllEmptyTableSlot => !tableSlots.Exists(x => x.assignedCustomer != null);
        public bool IsAllCustomerHasFood => customers.All(x => x.myFood != null);
        public int TableSlotCount => tableSlots.Count;

        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;

        public List<Customer> customers = new List<Customer>();

        public List<TableSlotData> tableSlots = new List<TableSlotData>();


        public void GroupCheck()
        {
            if (customers.Count == 2)
            {
                hasCustomerAssigned = true;
            }
        }


        public void HandleOrder()
        {
            foreach (TableSlotData tableSlot in tableSlots)
            {
                if (tableSlot.assignedCustomer != null)
                {
                    tableSlot.assignedCustomer.DecideMenu();
                    tableSlot.assignedCustomer.PatienceSliderReset();
                    tableSlot.assignedCustomer.SetCustomerState(CustomerState.WaitingFood);

                    IngameWaiterSystem.Instance.RemoveWaitingOrder(tableSlot.assignedCustomer);
                }
            }
        }

        public void AssignGroup(Customer customer)
        {
            isAlone = false;
            customers.Add(customer);
            GroupCheck();
        }

        public void AssignSingle()
        {
            hasCustomerAssigned = true;
            isAlone = true;
        }
        public void CustomerCheck()
        {
            if (IsAllEmptyTableSlot && CheckTableClean())
            {
                hasCustomerAssigned = false;

                IngameCustomerWaitingSystem.Instance.NotifyCanTableCheckIn();
            }
        }

        public override void Interact(CharacterBase character)
        {
            if (dishes.Count == 0 || character.item != null)
                return;

            character.item = dishes.Pop();
            character.item.transform.SetParent(character.transform);
            character.item.transform.position = character.spawnPos.position;
            CustomerCheck();
        }

        public bool CheckTableClean()
        {
            if (dishes.Count == 0)
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



