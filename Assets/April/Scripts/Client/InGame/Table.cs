using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace April
{

    public class Table : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.Table;

        public PlayerController player;

        private InteractionItem tableItem;

        private IButtonInteract buttonitem;

        public Transform spawnPoint;

        public Dish dish;
        //public float offset = 0.3f;
        private new void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (PlayerController.Instance.currentInteractionObject == this && buttonitem != null && PlayerController.Instance.isButtonPressed == true)
            {
                buttonitem.ButtonInteract();
            }
        }

        private void HandleTableInteractions()
        {
            if (player.item != null)
            {
                if (player.item is Dish)
                {
                    HandlePlayerHoldingDish();
                }
                else
                {
                    HandlePlayerHoldingOtherItem();
                }
            }
            else
            {
                HandlePlayerHoldingNothing();
            }
        }

        private void HandlePlayerHoldingDish()
        {
            if (tableItem is Food)
            {
                AddFoodToDish();
            }
            else
            {
                MoveDishToTable();
            }
        }

        private void HandlePlayerHoldingOtherItem()
        {
            if (tableItem is Dish)
            {
                AddPlayerItemToDish();
            }
            else
            {
                MoveItemToTable();
            }
        }

        private void HandlePlayerHoldingNothing()
        {
            if (tableItem != null)
            {
                MoveTableItemToPlayer();
            }
        }

        private void AddFoodToDish()
        {
            dish = player.item as Dish;
            var food = tableItem as Food;
            dish.AddItem(food, dish.spawnPoint.position);
            tableItem = null;
        }

        private void MoveDishToTable()
        {
            tableItem = player.item;
            dish = tableItem as Dish;
            dish.transform.SetParent(this.transform);
            dish.transform.position = spawnPoint.position;
            player.item = null;
        }

        private void AddPlayerItemToDish()
        {
            dish = tableItem as Dish;
            if (player.item is Ingredient)
            {
                var ingredient = player.item as Ingredient;
                if (ingredient.sliced == true)
                {
                    dish.AddItem(ingredient, dish.spawnPoint.position);
                }
            }
            else if (player.item is Food)
            {
                var food = player.item as Food;
                dish.AddItem(food, dish.spawnPoint.position);
            }
            player.item = null;
        }

        private void MoveItemToTable()
        {
            if (tableItem == null)
            {
                tableItem = player.item;
                if (tableItem is IButtonInteract)
                {
                    buttonitem = tableItem as IButtonInteract;
                    tableItem.ShowUI();
                }
                tableItem.transform.SetParent(this.transform);
                tableItem.transform.position = spawnPoint.position;
                player.item = null;
            }
        }


        private void MoveTableItemToPlayer()
        {
            if (tableItem is IButtonInteract)
            {
                buttonitem = tableItem as IButtonInteract;
                if ((int)buttonitem.ProgressValue != 0 && (int)buttonitem.ProgressValue != buttonitem.MaxValue)
                {
                    return;
                }
                tableItem.HideUI();
            }
            player.item = tableItem;
            tableItem.transform.SetParent(player.transform);
            tableItem.transform.position = player.spawnPos.position;
            tableItem = null;
        }





     

        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                HandleTableInteractions();
            }
        }

        public override void Exit()
        {

        }
    }
}