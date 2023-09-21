using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace April
{
    public class DishTable : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.DishTable;

        private CharacterBase character;
        private List<Dish> dishes = new List<Dish>();
        public Dish dish;
        [SerializeField] private int dishCount;
        public Transform spawnPoint;
        //public float offset = 0.3f;


        public override void Start()
        {
            base.Start();
            for (int i = 0; i < dishCount; i++)
            {
                Dish newDish = Instantiate(dish, this.transform);
                newDish.transform.position = spawnPoint.position;
                spawnPoint.position += new Vector3(0, dish.offset, 0);
                dishes.Add(newDish);
            }
        }

        void DishTableInteract()
        {
            if (character.item == null)
            {
                if (dishes.Count > 0)
                {
                    Dish dish = dishes[dishes.Count - 1];
                    dishes.RemoveAt(dishes.Count - 1);
                    dish.transform.SetParent(character.transform);
                    dish.transform.position = character.spawnPos.position;
                    spawnPoint.position -= new Vector3(0, dish.offset, 0);
                    character.item = dish;
                }
            }
            else
            {
                if (character.item is Dish)
                {
                    Dish dish = (Dish)character.item;
                    dishes.Add(dish);
                    dish.transform.SetParent(this.transform);
                    dish.transform.position = spawnPoint.position;
                    spawnPoint.position += new Vector3(0, dish.offset, 0);
                    character.item = null;
                }
                else if (character.item is Food)
                {
                    Food food = (Food)character.item;
                    Dish dish = dishes[dishes.Count - 1];
                    dishes.RemoveAt(dishes.Count - 1);
                    character.item = dish;
                    dish.transform.SetParent(character.transform);
                    dish.transform.position = character.spawnPos.position;
                    dish.AddItem(food, dish.spawnPoint.position);
                    spawnPoint.position -= new Vector3(0, dish.offset, 0);
                }
            }
        }

        public override void Interact(CharacterBase character)
        {
            this.character = character;
            if (this.character != null)
            {
                DishTableInteract();
            }
        }

        public override void Exit()
        {

        }
    }
}
