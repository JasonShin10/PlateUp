using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace April
{
    public class TrashCan : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.TrashCan;

        private PlayerController player;

        public Dish dish;

        void TrashCanInteract()
        {
            if (player.item.TryGetComponent(out Dish dish))
            {
                if (dish.ContainedFoodItems.Count > 0)
                {
                    Food foodItem = dish.ContainedFoodItems[0];

                   Destroy(foodItem.gameObject);
                    dish.ContainedFoodItems.Clear();
                    
                }
            }
        }
        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                TrashCanInteract();

            }
        }

        public override void Exit()
        {
            
        }
    }


}

