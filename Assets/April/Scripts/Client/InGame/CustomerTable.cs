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
        public Transform position;
    }

    public class CustomerTable : InteractionBase
    {
        public bool IsEmptyTable => !customerAssigned;

        public bool customerAssigned;
        public Stack<InteractionItem> dishes = new Stack<InteractionItem>();
        public override bool IsAutoInteractable => false;
        // 하나라도 존재하면 트루 아니면 false / 
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
                customerAssigned = true;
            }
        }

        public void ChangeCustomerState()
        {
            foreach (TableSlotData tableSlot in tableSlots)
            {
                if (tableSlot.assignedCustomer != null)
                {
                tableSlot.assignedCustomer.DecideMenu();
                tableSlot.assignedCustomer.PatienceSliderReset();
                tableSlot.assignedCustomer.SetCustomerState(CustomerState.WaitingFood);
                tableSlot.assignedCustomer.orderImageDisplay.gameObject.SetActive(true);

                IngameWaiterSystem.Instance.RemoveWaitingOrder(tableSlot.assignedCustomer);

                }




            }
        }

        public void CustomerCheck()
        {
            if (IsAllEmptyTableSlot && CheckTableClean())
            {
                customerAssigned = false;
                Debug.Log("CustomerCheck");
                IngameCustomerWaitingSystem.Instance.NotifyCanTableCheckIn();
            }
        }

        public override void Interact(CharacterBase character)
        {
            if (dishes.Count == 0 || character.item != null)
                return;

            character.item = dishes.Pop();
            character.item.transform.SetParent(character.transform);
            character.item.transform.localPosition = Vector3.up + Vector3.forward;
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



