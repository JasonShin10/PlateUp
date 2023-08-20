using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace April
{
    public class DishTable : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.DishTable;

        private PlayerController player;
        private List<Dish> dishes = new List<Dish>();
        public Dish dish;
        public float offset = 0.3f;

        public override void Start()
        {
            base.Start();
            for (int i = 0; i < 4; i++)
            {
                Dish newDish = Instantiate(dish, this.transform);
                newDish.transform.localPosition = new Vector3(0, offset, 0);
                offset += dish.offset;
                dishes.Add(newDish);
            }
        }

        void DishTableInteract()
        {
            if (player.item == null)
            {
                if (dishes.Count > 0)
                {
                    Dish dish = dishes[dishes.Count - 1];
                    dishes.RemoveAt(dishes.Count - 1);
                    dish.transform.SetParent(player.transform);
                    dish.transform.localPosition = Vector3.up + Vector3.forward;
                    offset -= dish.offset;
                    player.item = dish;
                }
            }
            else
            {
                if (player.item is Dish)
                {
                    Dish dish = (Dish)player.item;
                    dishes.Add(dish);
                    dish.transform.SetParent(this.transform);
                    dish.transform.localPosition = new Vector3(0, offset, 0);
                    offset += dish.offset;
                    player.item = null;
                }
                else if (player.item is Food)
                {
                    Food food = (Food)player.item;
                    Dish dish = dishes[dishes.Count - 1];
                    dishes.RemoveAt(dishes.Count - 1);
                    player.item = dish;
                    dish.transform.SetParent(player.transform);
                    dish.transform.localPosition = Vector3.up + Vector3.forward;
                    dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                    offset -= dish.offset;
                }
            }
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            DishTableInteract();
        }

        public override void Exit()
        {

        }
    }
}
