using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.UI;
using SysRandom = System.Random;

namespace April
{
    public class Customer : InteractionBase
    {
        public enum CustomerState
        {
            Entering,
            Waiting,
            WaitingOrder,
            WaitingFood,
            WaitingFriend,
            Leaving
        }

        public CustomerState currentCustomerState = CustomerState.Entering;
        public event Action OnCustomerCheckout;

        SysRandom random = new SysRandom();
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.None;

        public PlayerController player;

        public Chair target;
        public Transform exitTarget;
        public Food myFood;

        private InteractionItem foodDish;
        public Color GraphicColor
        {
            get => graphicRenderer.material.color;
            set => graphicRenderer.material.color = value;
        }

        private NavMeshAgent agent;
        public CustomerTable myTable;
        public TableSlotData mySeat;
        public Slider patienceSlider;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;
        public MenuImageContainer imageContainer;

        private float distanceBetweenDestination;
        private event Action onDestinationCallback;

        private MenuList orderedMenuType;
        private int orderedMenuStateType;

        public bool isGroup;
        //public bool waiting;
        public int waitingNum;
        public Transform waitingPos;

        protected override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);
            patienceSlider = GetComponentInChildren<Slider>(true);
            patienceSlider.maxValue = 90f;
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
            orderImageDisplay.gameObject.SetActive(false);
            OnCustomerCheckout += GoOut;



            SetCustomerState(currentCustomerState);
        }
        private void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, agent.destination);
            if (distanceBetweenDestination <= 0.1f)
            {
                onDestinationCallback?.Invoke();
                onDestinationCallback = null;
                if (currentCustomerState != CustomerState.Waiting)
                {
                    SetCustomerState(CustomerState.WaitingOrder);
                }
            }
        }

        public void SetCustomerState(CustomerState currentState)
        {
            switch (currentState)
            {
                case CustomerState.Entering:
                    currentCustomerState = CustomerState.Entering;
                    HandleEntering();
                    break;
                case CustomerState.Waiting:
                    currentCustomerState = CustomerState.Waiting;
                    HandleWaiting();
                    break;
                case CustomerState.WaitingOrder:
                    currentCustomerState = CustomerState.WaitingOrder;
                    HandleWaitingOrder();
                    break;
                case CustomerState.WaitingFood:
                    currentCustomerState = CustomerState.WaitingFood;
                    HandleWaitingFood();
                    break;
                case CustomerState.WaitingFriend:
                    currentCustomerState = CustomerState.WaitingFriend;
                    HandleWaitingFriend();
                    break;
                case CustomerState.Leaving:
                    currentCustomerState = CustomerState.Leaving;
                    HandleLeaving();
                    break;

            }
        }
        private void FindTarget()
        {
            if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
            {
                foreach (InteractionBase table in tables)
                {
                    var customerTable = table as CustomerTable;
                    if (customerTable.customerAssigned == true)
                    {
                        continue;
                    }

                    if (isGroup)
                    {
                        if (currentCustomerState != CustomerState.Waiting)
                        {
                            customerTable.customers.Add(this);
                        }
                        customerTable.GroupCheck();
                    }
                    else
                    {
                        customerTable.customerAssigned = true;
                    }
                    foreach (TableSlotData chairPos in customerTable.tableSlots)
                    {
                        if (chairPos.IsAssigned == true)
                        {
                            continue;
                        }
                        myTable = customerTable;
                        mySeat = chairPos;
                        //mySeat.SetAssigned(this);
                        mySeat.assignedCustomer = this;
                        MoveToTarget(mySeat.position, () =>
                        {

                            transform.LookAt(myTable.transform.position, Vector3.up);
                        });
                        return;
                    }
                }
            }


        }
        public Food GetFoodByMenu(MenuList menu, out Type menuStateType)
        {
            switch (menu)
            {
                case MenuList.Beef:
                    menuStateType = typeof(Beef.BeefState);
                    return new Beef();
                case MenuList.ThickBeef:
                    menuStateType = typeof(ThickBeef.ThickBeefState);
                    return new ThickBeef();
                default:
                    throw new ArgumentException("Invalid menu item");
            }
        }
        private void DecideMenu()
        {
            Array values = Enum.GetValues(typeof(MenuList));

            MenuList randomMenu = (MenuList)values.GetValue(random.Next(values.Length));
            GetFoodByMenu(randomMenu, out Type menuStateType);

            Array menuStates = Enum.GetValues(menuStateType);
            int firstState = (int)menuStates.GetValue(0);
            int lastState = (int)menuStates.GetValue(menuStates.Length - 1);

            int randomNum = UnityEngine.Random.Range(firstState, lastState);
            orderImageDisplay.sprite = imageContainer.sprites[0];



            //orderedMenuType = randomMenu;
            //orderedMenuStateType = randomNum;
            orderedMenuType = MenuList.Beef;
            orderedMenuStateType = 0;
        }

        private void HandleEntering()
        {
            FindTarget();
            //DecideMenu();
        }

        private void HandleWaiting()
        {
            //waitingCustomers.Add(this);
            MoveToTarget(waitingPos);

        }

        private void PatienceSliderActivate()
        {
            patienceSlider.gameObject.SetActive(true);
        }

        private void PatienceSliderInit()
        {
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
        }
        private void HandleWaitingOrder()
        {


            patienceSlider.value -= Time.deltaTime;
        }

        private void HandleWaitingFood()
        {

        }

        private void HandleWaitingFriend()
        {

        }
        public void GoOut()
        {
            //myTable.customerAssigned = false;
            mySeat.assignedCustomer = null;
            MoveToTarget(exitTarget.transform, () =>
            {
                IngameCustomerFactorySystem.Instance.RemoveCustomer(this);
            });
            //myTable.CustomerCheck();
        }

        private void HandleLeaving()
        {
            OnCustomerCheckout?.Invoke();
            IngameCustomerWaitingSystem.Instance.MakeCustomerForward();
        }
        public void MoveToTarget(Transform destination, Action callbackOnDestination = null)
        {

            onDestinationCallback += callbackOnDestination;
            if (destination == mySeat.position)
            {
                onDestinationCallback += PatienceSliderActivate;
            }
            agent.SetDestination(destination.position);
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            CustomerInteract();
        }

        IEnumerator Eat()
        {
            yield return new WaitForSeconds(maxTime);

            Dish emptyDish = (Dish)foodDish;
            emptyDish.RemoveItem(myFood);
            Destroy(myFood.gameObject);

            myFood = null;
            myTable.dishes.Push(foodDish);

            emptyDish.GetDirty();

            SetCustomerState(CustomerState.Leaving);
        }

        public void CustomerInteract()
        {
            if (currentCustomerState == CustomerState.WaitingOrder)
            {
                DecideMenu();
                currentCustomerState = CustomerState.WaitingFood;
                PatienceSliderInit();
                orderImageDisplay.gameObject.SetActive(true);

            }
            else if (currentCustomerState == CustomerState.WaitingFood)
            {
                if (player.item == null)
                    return;

                if (player.item.TryGetComponent(out Dish dish))
                {
                    if (dish.ContainedFoodItems.Count > 0)
                    {
                        Food foodItem = dish.ContainedFoodItems[0];
                        if (orderedMenuType == foodItem.MenuType && orderedMenuStateType == foodItem.CookingState)
                        {
                            orderImageDisplay.gameObject.SetActive(false);
                            myFood = foodItem;
                            foodDish = player.item;

                            player.item.transform.SetParent(myTable.transform);
                            player.item.transform.localPosition = new Vector3(0, 1.66f, 0);
                            player.item = null;

                            StartCoroutine(Eat());
                        }
                    }
                }
            }
        }

        float maxTime = 3f;

        public override void Exit()
        {

        }
    }
}

