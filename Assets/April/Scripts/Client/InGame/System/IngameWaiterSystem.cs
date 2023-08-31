using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameWaiterSystem : MonoBehaviour
    {
        public static IngameWaiterSystem Instance { get; private set; }

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

        public void NotifyWaitingOrder(Customer customer)
        {
            if (waitingOrderCustomerList.Contains(customer))
                return;

            waitingOrderCustomerList.Add(customer);

            var emptyJobWaiter = Character_Waitress.SpawnedWaitressList.Find(x => x.HasJobTask == false);
            if (emptyJobWaiter != null)
            {
                emptyJobWaiter.ReceiveCustomerOrder(customer);
            }
        }

        public void RemoveWaitingOrder(Customer customer)
        {
            waitingOrderCustomerList.Remove(customer);
        }

        public void NotifyWaitingFood(Customer customer)
        {
            if (waitingFoodCustomerList.Contains(customer))
                return;

            waitingFoodCustomerList.Add(customer);

            var emptyJobWaiter = Character_Waitress.SpawnedWaitressList.Find(x => x.HasJobTask == false);
            if (emptyJobWaiter != null)
            {
                emptyJobWaiter.HandleFoodArrived();
            }
        }

        public void RemoveWaitingFood(Customer customer)
        {
            waitingFoodCustomerList.Remove(customer);
        }
    }
}