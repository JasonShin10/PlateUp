using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace April
{
    public class Character_Waitress : CharacterBase
    {

        public override CharacterType CharacterType => CharacterType.Waitress;

        public Transform waitingPosition;
        
        private Customer currentTargetCustomer;
        [SerializeField]private List<Customer> waitingOrderCustomerList = new List<Customer>();
        [SerializeField]private List<Customer> waitingFoodCustomerList = new List<Customer>();

        [Button("Receive Customer Order")]
        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            this.SetDestination(currentTargetCustomer.transform.position, OnDestinationCustomer);
        }
        [Button("Find Customer")]

        public void FindCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;
            List<CustomerTable> tables = IngameCustomerFactorySystem.Instance.tables;
           
                foreach (Customer customer in table.customers)
                {
                    if ((int)customer.state < minStateValue)
                    {
                        minStateValue = (int)customer.state;
                    }
                    if (customer.patienceSlider.value < minPatienceValue)
                    {
                        minPatiecneCustomer = customer;
                    }
                }
            
            ReceiveCustomerOrder(minPatiecneCustomer);
        }
        protected override void Update()
        {
            base.Update();
            //Debug.Log(customerList.Count);
            // To do : Ray Casting For Customer
            // 1. if RayHit Customer ? => OnDestinationCustomer();
            // 2. Vector3.Distance(currentTargetCustomer.transform.position transform.position) <= 1f ? => OnDestinationCustomer();
            // 3. this.SetDestination(customer.transform.position, OnDestinationCustomer);
        }

        public void RegisterCustomer(Customer customer)
        {
            customer.OnStateChange += HandleStateChange;
        }

        private void HandleStateChange(CustomerState oldState, Customer customer)
        {
            switch (oldState)
            {
                case CustomerState.WaitingOrder:
                    waitingOrderCustomerList.Remove(customer);
                    break;
                case CustomerState.WaitingFood:
                    waitingFoodCustomerList.Remove(customer);
                    break;

            }
            switch (customer.State)
            {
                case CustomerState.WaitingOrder:
                    waitingOrderCustomerList.Add(customer);
                    break;
                case CustomerState.WaitingFood:
                    waitingFoodCustomerList.Add(customer);
                    break;
                case CustomerState.Leaving:
                    break;

            }
        }

        private void OnDestinationCustomer()
        {
            if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(null);
                currentTargetCustomer = null;

                this.SetDestination(waitingPosition.position);
            }
        }

    }
}

