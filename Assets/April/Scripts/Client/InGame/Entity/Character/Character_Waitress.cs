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
        public DishTable dishTable;
        private Customer currentTargetCustomer;
        public WaitressTable waitressTable;
        public Dish dish;
        public Food food;
        public TrashCan trashCan;

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
           
            hasJobTask = true;
            this.SetDestination(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
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
            else
            {
                hasJobTask = true;
                this.SetDestination(trashCan.transform.position);
                StartCoroutine(DelayedRegistDestinationCallback());
                IEnumerator DelayedRegistDestinationCallback()
                {
                    yield return new WaitForEndOfFrame();

                    OnDestination += OnWaitressArrivedTrashCan;
                }
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
                    this.SetDestination(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
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

        public void OnWaitressArrivedTrashCan()
        {
            trashCan.Interact(this);
            this.SetDestination(dishTable.transform.position);
            StartCoroutine(DelayedRegistDestinationCallback());
            IEnumerator DelayedRegistDestinationCallback()
            {
                yield return new WaitForEndOfFrame();

                OnDestination += OnWaitressArrivedDishTable;
            }
        }

        public void OnWaitressArrivedDishTable()
        {
            dishTable.Interact(this);
            this.SetDestination(waitingPosition.transform.position);
        }


        private void FindCustomer()
        {

        }
    }


}

