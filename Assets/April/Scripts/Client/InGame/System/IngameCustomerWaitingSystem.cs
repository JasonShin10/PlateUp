using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerWaitingSystem : MonoBehaviour
    {
        public static IngameCustomerWaitingSystem Instance { get; private set; } = null;

        // 비어있는 슬롯이 하나라도 있으면 true, 다 찼으면 false, 하지만 !니까 다찼을때 true;

        public bool IsFullWaitingSlot => !waitingSlots.Exists(x => x.customer == null);

        public bool IsWaitingExists => waitingSlots.Exists(x => x.customer != null);

        public List<IngameCustomerWaitingSystem_SlotData> waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>();

        private void Awake()
        {
            Instance = this;

            var slots = GetComponentsInChildren<IngameCustomerWaitingSystem_SlotData>();
            waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>(slots);
            firstSlot = waitingSlots[0];
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        IngameCustomerWaitingSystem_SlotData firstSlot;
        public void WaitCustomerComeIn()
        {
            
            if (firstSlot.customer == null)
            {
                return;
            }
            List<Customer> customers = new List<Customer>();

            if (firstSlot.customer.isGroup == true)
            {
                customers.Add(firstSlot.customer);
                foreach (var slot in waitingSlots)
                {
                    if (slot.customer == null) continue;
                    if (firstSlot != slot && firstSlot.customer.waitingNum == slot.customer.waitingNum)
                    {
                        customers.Add(slot.customer);
                        slot.customer = null;
                    }
                }
                firstSlot.customer = null;
                foreach (var customer in customers)
                {
                    customer.currentCustomerState = Customer.CustomerState.Entering;
                    customer.SetCustomerState(customer.currentCustomerState);
                }
            }
            else
            {
                firstSlot.customer.SetCustomerState(Customer.CustomerState.Entering);
                firstSlot.customer = null;
            }
        }
        public void MakeCustomerForward()
        {
            
            for (int i =0; i < waitingSlots.Count-1; i++)
            {
                if (waitingSlots[i].customer == null)
                {
                    if (waitingSlots[i+1].customer != null)
                    {
                        waitingSlots[i].customer = waitingSlots[i+1].customer;
                        waitingSlots[i].customer.waitingPos = waitingSlots[i].transform; 
                        waitingSlots[i].customer.SetCustomerState(Customer.CustomerState.Waiting);
                        waitingSlots[i+1].customer = null;
                    }
                }
            }
            if (IsWaitingExists)
            {
                if (waitingSlots[0].customer == null)
                {
                    MakeCustomerForward();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
               
            
 
            //foreach (var slot in waitingSlots)
            //{
            //    if (slot.customer == null)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        nextCustomer = slot.customer;
            //    }
            //}
            //foreach (var emptySlot in waitingSlots)
            //{
            //    if (emptySlot.customer == null)
            //    {
            //        emptySlot.customer = nextCustomer;
            //        nextCustomer = null;
            //    }
            //}


        }
    }

}

