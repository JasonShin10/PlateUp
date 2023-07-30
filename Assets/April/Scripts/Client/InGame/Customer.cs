using System;
using System.Collections;
using System.Collections.Generic;
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
        public override bool IsAutoInteractable => true;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.None;

        public PlayerController player;
        

        public Chair target;

        public Food orderFood;
        private Food foodComponent;

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
            var values = Enum.GetValues(typeof(MenuList));

            MenuList randomMenuy = (MenuList)values.GetValue(random.Next(values.Length));
            orderFood = GetFoodByMenu(randomMenuy);
            int randomNum = (int)randomMenuy;
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
        void Start()
        {
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
        // 손님에게 음식 가져다주기
        // 손님이 음식을 먹기
        // 떠나기
        public override void Interact(PlayerController player)
        {

        }

        public void CustomerInteract()
        {
            if (player.item != null)
            {
                orderFood = player.item;

                
                if (foodComponent.CookingState == orderFood.CookingState)
                {
                    foodComponent.transform.SetParent(this.transform);
                    foodComponent.transform.localPosition = Vector3.up;
                    foodComponent.gameObject.SetActive(true);
                    player.item = null;
                }
                Debug.Log("Item Insert To Table!");
            }
        }

        public override void Exit()
        {

        }
    }
}

