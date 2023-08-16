using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                   Destroy(foodItem);
                }
            }
        }
        public override void Interact(PlayerController player)
        {
            this.player = player;
            TrashCanInteract();
        }

        public override void Exit()
        {
            
        }
    }


}

