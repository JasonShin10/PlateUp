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

        private InteractionItem item;

        private IButtonInteract buttonitem;

        public float offset = 0.3f;
        private new void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (PlayerController.Instance.currentInteractionObject == this &&buttonitem != null && PlayerController.Instance.isButtonPressed == true)
            {
                buttonitem.ButtonInteract();
            }
        }

  
        void TableInteract()
        {
          
            if (player.item != null)
            {
                if (player.item is Dish)
                {
                   
                    if (item is Food)
                    {
                        var dish = player.item as Dish;
                        var food = item as Food;
                        dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                        item = null;
                        Debug.Log("Something is already on table");
                        return;
                    }
                    else
                    {
                        item = player.item;
                        var dish = item as Dish;
                        dish.transform.SetParent(this.transform);
                        dish.transform.localPosition = new Vector3(0, offset, 0);
                        player.item = null;
                        Debug.Log("Dish Insert To Table!");
                    }
                }
                else
                {
                    if (item is Dish)
                    {
                        var dish = item as Dish;
                        if (player.item is Ingredient)
                        {
                        var ingredient = player.item as Ingredient;
                            if (ingredient.sliced == true)
                            {
                                dish.AddItem(ingredient, new Vector3(0, offset, 0)); 
                            }
                        }
                        else if (player.item is Food)
                        {
                        var food = player.item as Food;
                        dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                        }
                        player.item = null;
                    }
                    else
                    {
                        if (item == null)
                        {
                            item = player.item;
                            if (item is IButtonInteract)
                            {
                                buttonitem = item as IButtonInteract;
                                buttonitem.ShowUI();
                            }
                            item.transform.SetParent(this.transform);
                            item.transform.localPosition = new Vector3(0, offset, 0);
                            player.item = null;
                            Debug.Log("Food Insert To Table!");
                        }

                    }

                }
            }
            else
            {
                if (item != null)
                {
                    if (item is IButtonInteract)
                    {
                        buttonitem = item as IButtonInteract;
                        if ((int)buttonitem.ProgressValue != 0 && (int)buttonitem.ProgressValue != buttonitem.MaxValue)
                        {
                            return;
                        }
                        buttonitem.HideUI();
                    }
                    player.item = item;
                    item.transform.SetParent(player.transform);
                    item.transform.localPosition = Vector3.up + Vector3.forward;
                    item = null;
                }
            }
        }

        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                TableInteract();
            }
        }

        public override void Exit()
        {

        }
    }
}