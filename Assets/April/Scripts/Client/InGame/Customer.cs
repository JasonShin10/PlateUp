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

        public event Action<CustomerState, Customer> OnStateChange;
        


        public CustomerState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    CustomerState oldState = state;
                    state = value;
                    OnStateChange?.Invoke(oldState, this);
                }
            }
        }


   
        public CustomerState state = CustomerState.Entering;

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

        public MenuList orderedMenuType;
        public int orderedMenuStateType;

        public bool isGroup;
        public int groupID;
        public Transform waitingPos;

        private int money = 100;
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

                if (state == CustomerState.WaitingOrder || state == CustomerState.WaitingFood || state == CustomerState.WaitingFriend)
                {
                    patienceSlider.value -= Time.deltaTime;
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
                        if (state != CustomerState.Waiting)
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
        public Food GetFoodByMenu(MenuList menu, out int firstState, out int lastState)
        {
            switch (menu)
            {
                case MenuList.Beef:
                    firstState = (int)Beef.BeefState.Raw;
                    lastState = (int)Beef.BeefState.WellDone;
                    return new Beef();
                case MenuList.ThickBeef:
                    firstState = (int)ThickBeef.ThickBeefState.Raw;
                    lastState = (int)ThickBeef.ThickBeefState.WellDone;
                    return new ThickBeef();
                default:
                    throw new ArgumentException("Invalid menu item");
            }
        }

        private void DecideMenu()
        {
            // Hint ? 
            MyEnumTypes randomType = (MyEnumTypes)UnityEngine.Random.Range((int)MyEnumTypes.None, (int)MyEnumTypes.RandomMax);

            Array values = Enum.GetValues(typeof(MenuList));
            MenuList randomMenu = (MenuList)values.GetValue(random.Next(values.Length));
            var food = GetFoodByMenu(randomMenu, out int firstState, out int lastState);
            int randomMenuInt = Convert.ToInt32(randomMenu);

            int randomMenuNum = UnityEngine.Random.Range(firstState, lastState + 1);

            orderImageDisplay.sprite = imageContainer.MenuSpriteGroups[randomMenuInt][randomMenuNum];
            orderedMenuType = 0;
            orderedMenuStateType = 0;
        }

        private void HandleEntering()
        {
            state = CustomerState.Entering;
            FindTarget();
        }

        private void HandleWaiting()
        {
            state = CustomerState.Waiting;
            
            MoveToTarget(waitingPos);
        }

        private void PatienceSliderActivate()
        {
            patienceSlider.gameObject.SetActive(true);
        }

        private void PatienceSliderReset()
        {
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
        }

        private void HandleWaitingOrder()
        {
            
            State = CustomerState.WaitingOrder;
            
            patienceSlider.value -= Time.deltaTime;
        }

        private void HandleWaitingFood()
        {
            State = CustomerState.WaitingFood;

            PatienceSliderActivate();
        }

        private void HandleWaitingFriend()
        {
            state = CustomerState.WaitingFriend;
            
            PatienceSliderReset();
            PatienceSliderActivate();
        }
        public void GoOut()
        {
            mySeat.assignedCustomer = null;
            MoveToTarget(exitTarget.transform, () =>
            {
                IngameCustomerFactorySystem.Instance.RemoveCustomer(this);
            });
        }

        private void HandleLeaving()
        {
            state = CustomerState.Leaving;
            GoOut();
            IngameUI.Instance.AddAssets(100);

        }

        public void MoveToTarget(Transform destination, Action callbackOnDestination = null)
        {
            onDestinationCallback += callbackOnDestination;
            if (destination == mySeat.position)
            {
                onDestinationCallback += PatienceSliderActivate;
                SetCustomerState(CustomerState.WaitingOrder);
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
            PatienceSliderReset();
            yield return new WaitForSeconds(maxTime);

            Dish emptyDish = (Dish)foodDish;
            emptyDish.RemoveItem(myFood);
            Destroy(myFood.gameObject);

            myFood = null;
            myTable.dishes.Push(foodDish);

            emptyDish.GetDirty();

            SetCustomerState(CustomerState.Leaving);
            myTable.customers.Clear();
        }

        IEnumerator Eat(IEnumerable<Customer> customers)
        {
            yield return new WaitForSeconds(maxTime);

            foreach (var customer in customers)
            {
                customer.PatienceSliderReset();
                Dish emptyDish = (Dish)customer.foodDish;
                emptyDish.RemoveItem(customer.myFood);
                Destroy(customer.myFood.gameObject);

                customer.myFood = null;
                customer.myTable.dishes.Push(customer.foodDish);

                emptyDish.GetDirty();

                customer.SetCustomerState(CustomerState.Leaving);
            }

            myTable.customers.Clear();
        }

        public void CustomerInteract()
        {
            if (state == CustomerState.WaitingOrder)
            {
                DecideMenu();
                PatienceSliderReset();
                SetCustomerState(CustomerState.WaitingFood);
                orderImageDisplay.gameObject.SetActive(true);
                return;
            }
            else if (state == CustomerState.WaitingFood || state == CustomerState.WaitingFriend)
            {
                if (player.item == null || myFood != null)
                {

                    return;
                }

                PatienceSliderReset();
                PatienceSliderActivate();
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
                            if (!isGroup)
                            {
                                StartCoroutine(Eat());
                            }
                            else if (myTable.IsAllCustomerHasFood)
                            {

                                StartCoroutine(Eat(myTable.customers));

                            }
                            else if (!myTable.IsAllCustomerHasFood)
                            {
                                foreach (var customer in myTable.customers)
                                {
                                    customer.SetCustomerState(CustomerState.WaitingFriend);

                                }
                            }

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
