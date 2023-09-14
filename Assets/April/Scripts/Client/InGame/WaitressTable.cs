using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class WaitressTable : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public bool HasFood => dish != null && dish.ContainedFoodItems.Count > 0;

        public override InteractionObjectType InterationObjectType => InteractionObjectType.WaitressTable;

        [field: SerializeField] public Transform InteractionPoint { get; private set; }

        private PlayerController player;
        private InteractionItem item;
        private Dish dish;
        private Food food;
        [SerializeField] private Transform spawnPoint; 
        //public float offset;
        public event Action OnFoodArrived;

        public void WaitressInteract(Character_Waitress waitress)
        {
            if (dish.ContainedFoodItems[0] != null && dish != null)
            {
                waitress.dish = dish;
                waitress.item = item;
                dish.transform.SetParent(waitress.transform);
                dish.transform.localPosition = Vector3.up + Vector3.forward;
                dish = null;
            }
        }

        void PlayerInteract()
        {
            if (player.item != null)
            {
                if (player.item is Dish)
                {
                    if (item is Food)
                    {
                        dish = player.item as Dish;
                        food = item as Food;
                        dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                        item = null;

                        OnFoodArrived?.Invoke();
                    }
                    else
                    {
                        item = player.item;
                        dish = item as Dish;
                        if (food)
                        {
                        food = dish.ContainedFoodItems[0];
                        }
                        dish.transform.SetParent(this.transform);
                        dish.transform.position = spawnPoint.position;
                        player.item = null;
                        OnFoodArrived?.Invoke();
                    }
                }
                else
                {
                    if (item is Dish)
                    {
                        dish = item as Dish;
                        food = player.item as Food;
                        dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                        player.item = null;
                    }
                    else
                    {
                        if (item == null)
                        {
                            item = player.item;
                            item.transform.SetParent(this.transform);
                            item.transform.position = spawnPoint.position;
                            player.item = null;
                        }

                    }

                }
            }
            else
            {
                if (item != null)
                {
                    player.item = item;
                    item.transform.SetParent(player.transform);
                    item.transform.localPosition = Vector3.up + Vector3.forward;
                    item = null;
                    food = null;
                    dish = null;
                }
            }
        }

        public override void Interact(CharacterBase character)
        {

            this.player = character as PlayerController;

            if (this.player != null)
            {
                PlayerInteract();

            }

        }



        public override void Exit()
        {

        }
    }
}