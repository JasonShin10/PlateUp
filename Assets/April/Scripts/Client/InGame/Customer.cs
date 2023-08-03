using System;
using System.Collections;
using System.Collections.Generic;
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

        public Food orderFood;
        public Food myFood;

        private InteractionItem foodDish;
        public Color GraphicColor
        {
            get => graphicRenderer.material.color;
            set => graphicRenderer.material.color = value;
        }

        private NavMeshAgent agent;
        private CustomerTable myTable;
        public Transform targetPosition;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;
        public MenuImageContainer imageContainer;

        protected override void Awake()
        {
            base.Awake();
            graphicRenderer = GetComponent<Renderer>();
        }
        public Food GetFoodByMenu(MenuList menu)
        {
            switch (menu)
            {
                case MenuList.Meat:
                    return new Meat();
                case MenuList.Chicken:
                    return new Chicken();
                default:
                    throw new ArgumentException("Invalid menu item");
            }
        }

        private void DecideMenu()
        {
            Array values = Enum.GetValues(typeof(MenuList));

            MenuList randomMenu = (MenuList)values.GetValue(random.Next(values.Length));
            orderFood = GetFoodByMenu(randomMenu);
            int randomNum = (int)randomMenu;
            orderImageDisplay.sprite = imageContainer.sprites[randomNum];
            orderImageDisplay.gameObject.SetActive(false);
        }


        private void FindTarget(List<InteractionBase> tables)
        {
            foreach (InteractionBase tableEntity in tables)
            {
                var customerTable = tableEntity as CustomerTable;
                if (customerTable.customerVisted == true)
                {
                    continue;
                }
                else
                {
                    myTable = customerTable;
                    foreach (Chair chair in customerTable.chiars)
                    {
                        if (chair.isVisited != true && chair.istargeted != true)

                        {
                            targetPosition = chair.transform;
                            
                            target = chair;
                            target.istargeted = true;
                            return;
                        }
                    }
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

            if (targetPosition != null)
            {
                MoveToTarget();
            }
        }

        // Update is called once per frame
        void MoveToTarget()
        {
            agent.SetDestination(targetPosition.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.transform == other.transform)
            {
                orderImageDisplay.gameObject.SetActive(true);
            }
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            CustomerInteract();

        }

        public void CustomerInteract()
        {
            if (player.item != null)
            {
                if (player.item.transform.childCount > 0 && player.item.transform.GetChild(0).GetComponent<Food>() != null)
                {
                    Food foodItem = player.item.transform.GetChild(0).GetComponent<Food>();
                    if (orderFood.GetType() == foodItem.GetType() && orderFood.CookingState == foodItem.CookingState)
                    {
                        orderImageDisplay.gameObject.SetActive(false);
                        myFood = foodItem;
                        foodDish = player.item;
                        
                        player.item.transform.SetParent(myTable.transform);
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
            myTable.dishes.Add(foodDish);
            Dish emptyDish = (Dish)foodDish;
            emptyDish.GetDirty();
            GoOut();
        }

        public void GoOut()
        {
            targetPosition = exitTarget;
            MoveToTarget();
        }
        public override void Exit()
        {

        }
    }
}

