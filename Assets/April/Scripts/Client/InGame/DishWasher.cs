using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class DishWasher : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;

        private PlayerController player;
        public Dish dish;
        void DishWasherInteract()
        {
            if (player.item != null)
            {
                dish = player.item as Dish;
                if (dish != null && dish.dirty == true)
                {
                    player.item.transform.SetParent(this.transform);
                    player.item.transform.localPosition = Vector3.up;
                    player.item = null;
                }
            }
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            DishWasherInteract();
        }

        public override void Exit()
        {

        }
    }


}

