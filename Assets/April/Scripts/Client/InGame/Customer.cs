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
        public CustomerTable_InteractSlot mySeat;
        public Slider patienceSlider;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;
        public MenuImageContainer imageContainer;

        private float distanceBetweenDestination;
        private event Action onDestinationCallback;

        private MenuList orderedMenuType;
        private int orderedMenuStateType;

        public bool isGroup;
        public bool waiting;
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

            if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
            {
                FindTarget(tables);
            }
        
            // 대기중인 손님에게 myTable을 어떻게 할당할까
            // 웨이팅 리스트에 넣어주고 
            // 웨이팅 리스트손님은 따로 ..

            SetCustomerState(currentCustomerState);
        }
        private void Update()
        {

            distanceBetweenDestination = Vector3.Distance(transform.position, agent.destination);
            if (distanceBetweenDestination <= 0.1f)
            {
                onDestinationCallback?.Invoke();
                onDestinationCallback = null;
                if (currentCustomerState != CustomerState.Waiting )
                {
                currentCustomerState = CustomerState.WaitingOrder;
                }
                SetCustomerState(currentCustomerState);
            }
        }

        void SetCustomerState(CustomerState currentCustomerState)
        {
            switch (currentCustomerState)
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

            }
        }

        private void HandleEntering()
        {
            DecideMenu();
        }

        private void HandleWaiting()
        {
            MoveToTarget(waitingPos.position);
        }

        private void HandleWaitingOrder()
        {
            patienceSlider.gameObject.SetActive(true);
            patienceSlider.value -= Time.deltaTime;
        }

        private void HandleWaitingFood()
        {

        }

        private void HandleWaitingFriend()
        {

        }

        private void Leaving()
        {

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
            
            orderImageDisplay.gameObject.SetActive(false);

            //orderedMenuType = randomMenu;
            //orderedMenuStateType = randomNum;
            orderedMenuType = MenuList.Beef;
            orderedMenuStateType = 0;
        }


        private void FindTarget(List<InteractionBase> tableSlots)
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
                    foreach (CustomerTable_InteractSlot chairPos in customerTable.chairPos)
                    {
                        if (chairPos.customerAssigned == true)
                        {
                            continue;
                        }
                        myTable = customerTable;
                        mySeat = chairPos;
                        mySeat.customerAssigned = true;
                        MoveToTarget(chairPos.transform.position, () =>
                        {
                            orderImageDisplay.gameObject.SetActive(true);
                            transform.LookAt(myTable.transform.position, Vector3.up);
                        });
                        return;
                    }
                }
            }
           
  
        }


        // Update is called once per frame
        public void MoveToTarget(Vector3 destination, Action callbackOnDestination = null)
        {
            onDestinationCallback += callbackOnDestination;
            agent.SetDestination(destination);
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            CustomerInteract();
        }

        public void CustomerInteract()
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

        float maxTime = 3f;
        IEnumerator Eat()
        {
            yield return new WaitForSeconds(maxTime);

            Destroy(myFood.gameObject);

            myFood = null;
            myTable.customerAssigned = false;
            myTable.item = foodDish;

            Dish emptyDish = (Dish)foodDish;
            emptyDish.GetDirty();

            GoOut();
        }

        public void GoOut()
        {
            MoveToTarget(exitTarget.transform.position, () =>
            {
                CustomerFactory.Instance.RemoveCustomer(this);
            });
        }

        public override void Exit()
        {

        }
    }
}

