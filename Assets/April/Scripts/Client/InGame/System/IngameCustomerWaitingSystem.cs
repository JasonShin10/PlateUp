using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerWaitingSystem : MonoBehaviour
    {
        public static IngameCustomerWaitingSystem Instance { get; private set; }

        public bool IsFullWaitingSlot => !waitingSlots.Exists(x => x.customer == null);
        public int EmptyWaitingSlotCount
        {
            get
            {
                int result = 0;
                foreach (var slot in waitingSlots)
                {
                    if (!slot.IsExistCustomer)
                        result++;
                }
                return result;
            }
        }


        public List<IngameCustomerWaitingSystem_SlotData> waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>();

        private int waitingIdentifyIncreament = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public bool TryGetEmptySlots(int requireSlotCount, out int id, out List<IngameCustomerWaitingSystem_SlotData> slots)
        {
            int length = Mathf.Min(requireSlotCount, waitingSlots.Count);
            slots = new List<IngameCustomerWaitingSystem_SlotData>();
            for (int i = 0; i < waitingSlots.Count; i++)
            {
                if (slots.Count >= length)
                    break;

                if (!waitingSlots[i].IsExistCustomer)
                {
                    slots.Add(waitingSlots[i]);
                }
            }

            if (slots.Count > 0)
            {
                id = waitingIdentifyIncreament++;
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }

        //public bool TryGetEmptySlot(out int id, out IngameCustomerWaitingSystem_SlotData slot)
        //{
        //    for (int i = 0; i < waitingSlots.Count; i++)
        //    {
        //        if (waitingSlots[i].customer == null)
        //        {
        //            id = waitingIdentifyIncreament++;
        //            slot = waitingSlots[i];
        //            return true;
        //        }
        //    }

        //    id = -1;
        //    slot = null;
        //    return false;
        //}

        public void NotifyCanTableCheckIn()
        {           
            var firstSlot = waitingSlots[0];
            if (!firstSlot.IsExistCustomer)
                return;

            int targetGroupID = firstSlot.customer.groupID;
            if (Customer.TryGetCustomerGroup(targetGroupID, out var waitingCustomers))
            {
                waitingCustomers.ForEach(customer =>
                {                   
                    customer.SetCustomerState(CustomerState.Entering);
                });
            }

            int groupEndSlotIndex = waitingCustomers.Count;
            for (int i = 0; i < groupEndSlotIndex; i++)
            {
                waitingSlots[i].customer = null;
            }
            ShiftWaitingCustomers();
        }

        public void CheckWaitingCustomerPossibleEnter()
        {
            if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
            {
                foreach (InteractionBase table in tables)
                {
                    var customerTable = table as CustomerTable;
                    if (customerTable.IsEmptyTable)
                    {
                        NotifyCanTableCheckIn();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        public void ShiftWaitingCustomers()
        {
            for (int i = 0; i < waitingSlots.Count; i++)
            {
                if (waitingSlots[i].IsExistCustomer)
                {
                    var emptySlot = waitingSlots.Find(x => !x.IsExistCustomer);
                    waitingSlots[i].customer.waitingPos = emptySlot.transform;
                    emptySlot.customer = waitingSlots[i].customer;
                    emptySlot.customer.waitingPos = emptySlot.transform;
                    emptySlot.customer.SetCustomerState(CustomerState.Waiting);
                    waitingSlots[i].customer = null;
                }
            }
        }

        #region OLD CODE

        //public class IngameCustomerWaitingSystem : MonoBehaviour
        //{
        //    public static IngameCustomerWaitingSystem Instance { get; private set; } = null;

        //    // 비어있는 슬롯이 하나라도 있으면 true, 다 찼으면 false, 하지만 !니까 다찼을때 true;

        //    public bool IsFullWaitingSlot => !waitingSlots.Exists(x => x.customer == null);

        //    public bool IsWaitingExists => waitingSlots.Exists(x => x.customer != null);

        //    public List<IngameCustomerWaitingSystem_SlotData> waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>();

        //    private void Awake()
        //    {
        //        Instance = this;

        //        var slots = GetComponentsInChildren<IngameCustomerWaitingSystem_SlotData>();
        //        waitingSlots = new List<IngameCustomerWaitingSystem_SlotData>(slots);
        //        firstSlot = waitingSlots[0];
        //    }

        //    private void OnDestroy()
        //    {
        //        Instance = null;
        //    }

        //    IngameCustomerWaitingSystem_SlotData firstSlot;

        //    public void WaitCustomerComeIn()
        //    {

        //        if (firstSlot.customer == null)
        //        {
        //            return;
        //        }
        //        List<Customer> customers = new List<Customer>();

        //        if (firstSlot.customer.isGroup == true)
        //        {
        //            customers.Add(firstSlot.customer);
        //            foreach (var slot in waitingSlots)
        //            {
        //                if (slot.customer == null) continue;
        //                if (firstSlot != slot && firstSlot.customer.waitingNum == slot.customer.waitingNum)
        //                {
        //                    customers.Add(slot.customer);
        //                    slot.customer = null;
        //                }
        //            }
        //            firstSlot.customer = null;
        //            foreach (var customer in customers)
        //            {
        //                customer.currentCustomerState = Customer.CustomerState.Entering;
        //                customer.SetCustomerState(customer.currentCustomerState);
        //            }
        //        }
        //        else
        //        {
        //            firstSlot.customer.SetCustomerState(Customer.CustomerState.Entering);
        //            firstSlot.customer = null;
        //        }
        //    }
        //    public void MakeCustomerForward()
        //    {

        //        for (int i =0; i < waitingSlots.Count-1; i++)
        //        {
        //            if (waitingSlots[i].customer == null)
        //            {
        //                if (waitingSlots[i+1].customer != null)
        //                {
        //                    waitingSlots[i].customer = waitingSlots[i+1].customer;
        //                    waitingSlots[i].customer.waitingPos = waitingSlots[i].transform; 
        //                    waitingSlots[i].customer.SetCustomerState(Customer.CustomerState.Waiting);
        //                    waitingSlots[i+1].customer = null;
        //                }
        //            }
        //        }
        //        if (IsWaitingExists)
        //        {
        //            if (waitingSlots[0].customer == null)
        //            {
        //                MakeCustomerForward();
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }



        //        //foreach (var slot in waitingSlots)
        //        //{
        //        //    if (slot.customer == null)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    else
        //        //    {
        //        //        nextCustomer = slot.customer;
        //        //    }
        //        //}
        //        //foreach (var emptySlot in waitingSlots)
        //        //{
        //        //    if (emptySlot.customer == null)
        //        //    {
        //        //        emptySlot.customer = nextCustomer;
        //        //        nextCustomer = null;
        //        //    }
        //        //}


        //    }
        //}
        #endregion
    }
}
