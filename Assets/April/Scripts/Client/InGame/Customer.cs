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
        private CustomerTable_InteractSlot myTableSlot;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;
        public MenuImageContainer imageContainer;

        private float distanceBetweenDestination;
        private event Action onDestinationCallback;

        private MenuList orderedMenuType;
        private int orderedMenuStateType;

        protected override void Awake()
        {
            base.Awake();
        }

        public Food GetFoodByMenu(MenuList menu, out Type menuStateType)
        {
            switch (menu)
            {
                case MenuList.Meat:
                    menuStateType = typeof(Meat.MeatState);
                    return new Meat();
                case MenuList.Chicken:
                    menuStateType = typeof(Chicken.ChickenState);
                    return new Chicken();
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
            orderedMenuType = MenuList.Meat;
            orderedMenuStateType = 0;
        }


        private void FindTarget(List<InteractionBase> tableSlots)
        {
            foreach (InteractionBase tableSlotEntity in tableSlots)
            {
                var customerTableSlot = tableSlotEntity as CustomerTable_InteractSlot;
                if (customerTableSlot.customerAssigned == true)
                {
                    continue;
                }
                else
                {
                    myTableSlot = customerTableSlot;
                    myTableSlot.customerAssigned = true;
                    MoveToTarget(customerTableSlot.transform.position, () =>
                    {
                        orderImageDisplay.gameObject.SetActive(true);
                        transform.LookAt(myTableSlot.parentTable.transform.position, Vector3.up);
                    });
                    break;
                }
            }
        }
        new void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);

            DecideMenu();

            if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
            {
                FindTarget(tables);
            }
        }

        private void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, agent.destination);
            if (distanceBetweenDestination <= 0.1f)
            {
                onDestinationCallback?.Invoke();
                onDestinationCallback = null;
            }
        }

        // Update is called once per frame
        void MoveToTarget(Vector3 destination, Action callbackOnDestination = null)
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

                        player.item.transform.SetParent(myTableSlot.transform);
                        player.item.transform.localPosition = Vector3.up;
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
            myTableSlot.customerAssigned = false;
            myTableSlot.item = foodDish;

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

