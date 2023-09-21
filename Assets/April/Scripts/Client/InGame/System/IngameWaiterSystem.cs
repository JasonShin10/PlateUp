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

        //public void CheckCustomer()
        //{
        //    if (waitingOrderCustomerList.Count > 0)
        //    {
        //        var emptyJobwaiter = Character_Waitress.SpawnedWaitressList.Find(x => x.HasJobTask == false);
        //        foreach (var customer in waitingOrderCustomerList)
        //        {
        //            if (emptyJobwaiter != null)
        //            {
        //                emptyJobwaiter.ReceiveCustomerOrder(customer);
        //            }
        //        }
        //    }
        //}

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

            //var emptyJobWaiter = Character_Waitress.SpawnedWaitressList.Find(x => x.HasJobTask == false);
            //if (emptyJobWaiter != null)
            //{
            //    emptyJobWaiter.HandleFoodArrived();
            //}
        }

        public void RemoveWaitingFood(Customer customer)
        {
            waitingFoodCustomerList.Remove(customer);
        }

        public void RemoveCustomer(Customer customer)
        {
            if ( waitingFoodCustomerList.Contains(customer))
            {
                waitingFoodCustomerList.Remove(customer);
            }
            else if ( waitingOrderCustomerList.Contains(customer))
            {
                waitingOrderCustomerList.Remove(customer);
            }
        }
    }
}