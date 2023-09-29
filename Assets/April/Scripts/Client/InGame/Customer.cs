using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.UI;
using SysRandom = System.Random;

namespace April
{
    public enum CustomerJobTypes
    {
        Rich,
        Bagger,
        Worker,
        Doctor,
    }

    public class Customer : CharacterBase
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

        [field: SerializeField] public CustomerData RuntimeCustomerData { get; set; }

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


        public override bool IsAutoInteractable => false;
        public override CharacterType CharacterType => CharacterType.Waitress;

        public CustomerJobTypes CustomerJobType { get; set; }

        private NavMeshAgent agent;
        private CharacterBase character;

        public Transform exitTarget;
        public Transform waitingPos;

        public CustomerTable myTable;
        public TableSlotData mySeat;


        private event Action onDestinationCallback;

        public static event Action OnLooseLife;
        public MenuList orderedMenuType;
        public int orderedMenuStateType;

        public bool isGroup;
        public int groupID;

        public bool patience = true;
        private int money = 100;

        [Title("InteractionItem")]
        public Food orderFood;
        public Food myFood;
        private InteractionItem foodDish;

        [Title("UI")]
        public Slider patienceSlider;
        public Image orderImageDisplay;
        public Image speechBubble;
        public Image angryEmoji;
        public TextMeshProUGUI text;
        public SpeechBubbleTextContainer textContainer;
        public MenuImageContainer imageContainer;

        [Title("Visualization")]
        public Transform graphicRoot;
        public VisualizationCharacter visualization;

        protected override void Awake()
        {
            SpawnedCustomers.Add(this);
            base.Awake();

            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);
            
        }

        public override void Start()
        {
            base.Start();
            int RandomNum = UnityEngine.Random.Range(0, textContainer.textContainer.Count);
            text.text = textContainer.textContainer[RandomNum];
            patienceSlider.maxValue = 360f;
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
            orderImageDisplay.gameObject.SetActive(false);
        }

        protected override void OnDestroy()
        {
            SpawnedCustomers.Remove(this);

            base.OnDestroy();
        }

        protected override void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, agent.destination);
            if (distanceBetweenDestination <= 1f)
            {
                moving = false;
                NavAgent.isStopped = true;
                NavAgent.destination = transform.position;
                        NavAgent.isStopped = false;
                onDestinationCallback?.Invoke();
                if (state == CustomerState.Entering)
                {
                    StartCoroutine(DelayedRotateFixing());
                    IEnumerator DelayedRotateFixing()
                    {
                        yield return new WaitForSeconds(0.1f);

                        transform.position = mySeat.seatTransform.position;
                        transform.LookAt(myTable.transform, Vector3.up);
                        visualization.SetInteractionSit(true);
                    }
                    SetCustomerState(CustomerState.WaitingOrder);
                }
                onDestinationCallback = null;

            }
            if (state == CustomerState.WaitingOrder || state == CustomerState.WaitingFood || state == CustomerState.WaitingFriend)
            {
                patienceSlider.value -= RuntimeCustomerData.PaitenceValue * Time.deltaTime;
            }
            if (moving == true)
            {
                visualization.SetMovement(0.5f);
            }
            else
            {
                visualization.SetMovement(0f);
            }

            if (patienceSlider.value <= 0f)
            {
                patience = false;
                SetCustomerState(CustomerState.Leaving);
               
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
                        customerTable.alone = false;
                        if (state != CustomerState.Waiting)
                        {
                            customerTable.customers.Add(this);
                        }
                        customerTable.GroupCheck();
                    }
                    else
                    {
                        customerTable.customerAssigned = true;
                        customerTable.alone = true;
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
                case MenuList.Salad:
                    firstState = 1;
                    lastState = 1;
                    return new Salad();
                default:
                    throw new ArgumentException("Invalid menu item");
            }
        }

        public void DecideMenu()
        {
            
            //MyEnumTypes randomType = (MyEnumTypes)UnityEngine.Random.Range((int)MyEnumTypes.None, (int)MyEnumTypes.RandomMax);

            Array values = Enum.GetValues(typeof(MenuList));
            MenuList randomMenu = (MenuList)UnityEngine.Random.Range((int)MenuList.Beef, (int)MenuList.Salad + 1);
            
            var food = GetFoodByMenu(randomMenu, out int firstState, out int lastState);
            int randomMenuInt = Convert.ToInt32(randomMenu);

            int randomMenuNum = UnityEngine.Random.Range(firstState, lastState + 1);

            orderImageDisplay.sprite = imageContainer.MenuSpriteGroups[randomMenuInt][randomMenuNum-1];
            orderImageDisplay.SetNativeSize();
            //orderedMenuType = 0;
            //orderedMenuStateType = 0;
            orderedMenuType = randomMenu;
            orderedMenuStateType = randomMenuNum; 
        }

        private void HandleEntering()
        {
            state = CustomerState.Entering;
            FindTarget();
        }

        private void HandleWaiting()
        {
            state = CustomerState.Waiting;
            moving = false;
            MoveToTarget(waitingPos);
        }


        private void PatienceSliderActivate()
        {
            patienceSlider.gameObject.SetActive(true);
        }

        public void PatienceSliderReset()
        {
            patienceSlider.value = patienceSlider.maxValue;
            patienceSlider.gameObject.SetActive(false);
        }

        private void HandleWaitingOrder()
        {
            State = CustomerState.WaitingOrder;
            mySeat.arrivedCustomer = this;
            if (myTable.alone == true)
            {
                IngameWaiterSystem.Instance.NotifyWaitingOrder(this);
            }
            else if (myTable.IsAllCustomerArrived)
            {
                IngameWaiterSystem.Instance.NotifyWaitingOrder(this);
            }

            patienceSlider.value -=  Time.deltaTime;
        }

        private void HandleWaitingFood()
        {
            State = CustomerState.WaitingFood;

            IngameWaiterSystem.Instance.NotifyWaitingFood(this);

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
            MoveToTarget(exitTarget.transform, () =>
            {
                IngameCustomerFactorySystem.Instance.RemoveCustomer(this);
            });
        }
        private void ResetTableVariables()
        {
            mySeat.arrivedCustomer = null;
            mySeat.assignedCustomer = null;

        }
        private void HandleLeaving()
        {
            state = CustomerState.Leaving;
            visualization.SetInteractionSit(false);
            PatienceSliderReset();
            ResetTableVariables();
            myTable.CustomerCheck();

            if (patience == false)
            {
                OnLooseLife?.Invoke();
                IngameWaiterSystem.Instance.RemoveCustomer(this);
                GoOut();
                StartCoroutine(ActivateSpeechBubble());
                orderImageDisplay.gameObject.SetActive(false);
            }
            else
            {
                IngameWaiterSystem.Instance.RemoveCustomer(this);
                GoOut();
                IngameUI.Instance.AddAssets(100);
                StartCoroutine(ActivateSpeechBubble());
            }
        }

        public void MoveToTarget(Transform destination, Action callbackOnDestination = null)
        {
            onDestinationCallback += callbackOnDestination;
            agent.SetDestination(destination.position);
            moving = true;
            if (destination == mySeat.position)
            {
                onDestinationCallback += PatienceSliderActivate;
            }
        }

        public override void Interact(CharacterBase character)
        {
            this.character = character;
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
        IEnumerator ActivateSpeechBubble()
        {
            speechBubble.gameObject.SetActive(true);
            if (patience == false)
            {
                text.gameObject.SetActive(false);
                angryEmoji.gameObject.SetActive(true);
            }
            else
            {
                text.gameObject.SetActive(true);
                angryEmoji.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(maxTime);
            speechBubble.gameObject.SetActive(false);
        }

        public void CustomerInteract()
        {
            if (state == CustomerState.WaitingOrder)
            {
                myTable.ChangeCustomerState();
                //DecideMenu();
                //PatienceSliderReset();
                //SetCustomerState(CustomerState.WaitingFood);
                //orderImageDisplay.gameObject.SetActive(true);

                //IngameWaiterSystem.Instance.RemoveWaitingOrder(this);

                return;
            }
            else if (state == CustomerState.WaitingFood || state == CustomerState.WaitingFriend)
            {
                if (character.item == null || myFood != null)
                {
                    return;
                }

                IngameWaiterSystem.Instance.RemoveWaitingFood(this);

                PatienceSliderReset();
                PatienceSliderActivate();
                if (character.item.TryGetComponent(out Dish dish))
                {
                    if (dish.ContainedFoodItems.Count > 0)
                    {
                        Food foodItem = dish.ContainedFoodItems[0];
                        if (orderedMenuType == foodItem.MenuType && orderedMenuStateType == foodItem.CookingState)
                        {
                            orderImageDisplay.gameObject.SetActive(false);
                            myFood = foodItem;
                            foodDish = character.item;
                            character.item.transform.SetParent(myTable.transform);
                            character.item.transform.localPosition = new Vector3(0, 1.66f, 0);
                            character.item = null;
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

    }
}
