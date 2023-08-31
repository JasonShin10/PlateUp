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
        public static List<Character_Waitress> SpawnedWaitressList = new List<Character_Waitress>();
        public override CharacterType CharacterType => CharacterType.Waitress;
        public Transform waitingPosition;
        private Customer currentTargetCustomer;
        public WaitressTable waitressTable;
        public Dish dish;
        public Food food;
        bool hasFood =false;
        public int foodState;
        
        protected override void Awake()
        {
            base.Awake();
            SpawnedWaitressList.Add(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SpawnedWaitressList.Remove(this);
        }

        [Button("Receive Customer Order")]
        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            this.SetDestination(currentTargetCustomer.transform.position, OnDestinationCustomer);
        }

        public override void Start()
        {
            base.Start();
            waitressTable.OnFoodArrived += HandleFoodArrived;
        }

        public void HandleFoodArrived()
        {
            hasFood = true;
            this.SetDestination(waitressTable.transform.position, OnWaitressTable);
        }


        public void FindWaitingOrderCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;

            foreach (Customer customer in IngameWaiterSystem.Instance.waitingOrderCustomerList)
            {
                if ((int)customer.state < minStateValue)
                {
                    minStateValue = (int)customer.state;
                }
                if (customer.patienceSlider.value < minPatienceValue)
                {
                    minPatienceValue = customer.patienceSlider.value;
                    minPatiecneCustomer = customer;
                }
            }
            ReceiveCustomerOrder(minPatiecneCustomer);
        }

        public void FindWaitingFoodCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;

            foreach (Customer customer in IngameWaiterSystem.Instance.waitingFoodCustomerList)
            {
                if (customer.orderedMenuType == food.MenuType && customer.orderedMenuStateType == food.CookingState)
                {
                    if ((int)customer.state < minStateValue)
                    {
                        minStateValue = (int)customer.state;
                    }
                    if (customer.patienceSlider.value < minPatienceValue)
                    {
                        minPatienceValue = customer.patienceSlider.value;
                        minPatiecneCustomer = customer;
                    }
                }
            }
            if (minPatiecneCustomer)
            {
            ReceiveCustomerOrder(minPatiecneCustomer);
                Debug.Log(minPatiecneCustomer.name);
            }
        }
        protected override void Update()
        {
            base.Update();
            //Debug.Log(customerList.Count);
            // To do : Ray Casting For Customer
            // 1. if RayHit Customer ? => OnDestinationCustomer();
            // 2. Vector3.Distance(currentTargetCustomer.transform.position transform.position) <= 1f ? => OnDestinationCustomer();
            // 3. this.SetDestination(customer.transform.position, OnDestinationCustomer);
            if (IngameWaiterSystem.Instance.waitingOrderCustomerList.Count >0 && hasFood == false)
            {
                FindWaitingOrderCustomer();
            }
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
                    IngameWaiterSystem.Instance.waitingOrderCustomerList.Remove(customer);
                    break;
                case CustomerState.WaitingFood:
                    IngameWaiterSystem.Instance.waitingFoodCustomerList.Remove(customer);
                    break;
                case CustomerState.Leaving:
                    IngameWaiterSystem.Instance.waitingOrderCustomerList.Remove(customer);
                    IngameWaiterSystem.Instance.waitingFoodCustomerList.Remove(customer);
                    break;

            }
            switch (customer.State)
            {
                case CustomerState.WaitingOrder:
                    IngameWaiterSystem.Instance.waitingOrderCustomerList.Add(customer);
                    break;
                case CustomerState.WaitingFood:
                    IngameWaiterSystem.Instance.waitingFoodCustomerList.Add(customer);
                    break;
                case CustomerState.Leaving:
                    break;

            }
        }
        private void OnDestinationCustomer()
        {
            Debug.Log(currentTargetCustomer.name);
         if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(this);
                currentTargetCustomer = null;

                this.SetDestination(waitingPosition.position);
            }
        }

        private void OnWaitressTable()
        {
            waitressTable.waitressInteract(this);
            if (dish)
            {
            food = dish.ContainedFoodItems[0];
            foodState = food.CookingState;
                FindWaitingFoodCustomer();
            }
            
        }

        private void FindCustomer()
        {

        }
    }


}

