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

        public bool HasJobTask => hasJobTask;

        public Transform waitingPosition;
        private Customer currentTargetCustomer;
        public WaitressTable waitressTable;
        public Dish dish;
        public Food food;
        bool hasFood = false;
        public int foodState;

        private bool hasJobTask = false;

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

        public void ReceiveCustomerOrder(Customer customer)
        {
            hasJobTask = true;
            currentTargetCustomer = customer;
            this.SetDestination(currentTargetCustomer.transform.position);

            StartCoroutine(DelayedRegistDestinationCallback());
            IEnumerator DelayedRegistDestinationCallback()
            {
                yield return new WaitForEndOfFrame();

                OnDestination += OnDestinationCustomer;
            }
            
        }

        public override void Start()
        {
            base.Start();
            waitressTable.OnFoodArrived += HandleFoodArrived;
        }

        public void HandleFoodArrived()
        {
            hasFood = true;
            hasJobTask = true;
            this.SetDestination(waitressTable.IneractionPoint.position, OnWaitressArrivedTable);
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
            if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(this);
                currentTargetCustomer = null;

                if (IngameWaiterSystem.Instance.waitingOrderCustomerList.Count > 0)
                {
                    ReceiveCustomerOrder(IngameWaiterSystem.Instance.waitingOrderCustomerList[0]);
                }
                else if (IngameWaiterSystem.Instance.waitingFoodCustomerList.Count > 0 && waitressTable.HasFood)
                {
                    this.SetDestination(waitressTable.IneractionPoint.position, OnWaitressArrivedTable);
                }
                else
                {
                    this.SetDestination(waitingPosition.position);
                    hasJobTask = false;
                }
            }
        }

        private void OnWaitressArrivedTable()
        {
            waitressTable.WaitressInteract(this);
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

