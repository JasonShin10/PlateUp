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
        public static List<Customer> SpawnedCustomers = new List<Customer>();

        public static bool TryGetCustomerGroup(int groupId, out List<Customer> result)
        {
            result = new List<Customer>();
            for (int i = 0; i < SpawnedCustomers.Count; i++)
            {
                if (SpawnedCustomers[i].groupID == groupId)
                {
                    result.Add(SpawnedCustomers[i]);
                }
            }
            return result.Count > 0;
        }


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
        public int groupID;
        public Transform waitingPos;

        protected override void Awake()
        {
            SpawnedCustomers.Add(this);
            base.Awake();

            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);
            patienceSlider = GetComponentInChildren<Slider>(true);
        }

        public override void Start()
        {
            base.Start();
            patienceSlider.maxValue = 90f;
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
            orderImageDisplay.gameObject.SetActive(false);
            OnCustomerCheckout += GoOut;
            //OnCustomerCheckout += IngameCustomerWaitingSystem.Instance.MakeCustomerForward;

            //SetCustomerState(currentCustomerState);
        }

        protected override void OnDestroy()
        {
            SpawnedCustomers.Remove(this);

            base.OnDestroy();
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
                    
                    HandleEntering();
                    break;
                case CustomerState.Waiting:

                    HandleWaiting();
                    break;
                case CustomerState.WaitingOrder:

                    HandleWaitingOrder();
                    break;
                case CustomerState.WaitingFood:
                    
                    HandleWaitingFood();
                    break;
                case CustomerState.WaitingFriend:
                   
                    HandleWaitingFriend();
                    break;
                case CustomerState.Leaving:
                    
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
                    if (!customerTable.IsEmptyTable)
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
            currentCustomerState = CustomerState.Entering;
            FindTarget();
            Debug.Log("999");
            //DecideMenu();
        }

        private void HandleWaiting()
        {
            currentCustomerState = CustomerState.Waiting;
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

            currentCustomerState = CustomerState.WaitingOrder;
            patienceSlider.value -= Time.deltaTime;
        }

        private void HandleWaitingFood()
        {
            currentCustomerState = CustomerState.WaitingFood;
        }

        private void HandleWaitingFriend()
        {
            currentCustomerState = CustomerState.WaitingFriend;
        }
        public void GoOut()
        {
            //myTable.customerAssigned = false;
            myTable.customers.Clear();
            mySeat.assignedCustomer = null;
            MoveToTarget(exitTarget.transform, () =>
            {
                IngameCustomerFactorySystem.Instance.RemoveCustomer(this);
            });
            //myTable.CustomerCheck();
        }

        private void HandleLeaving()
        {
            currentCustomerState = CustomerState.Leaving;
            OnCustomerCheckout?.Invoke();
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
            if (currentCustomerState == CustomerState.WaitingFood)
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

