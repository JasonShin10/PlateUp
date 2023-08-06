using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace April
{
    public class DishWasher : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.DishWasher;

        private PlayerController player;
        public Dish dish;

        public void Update()
        {
            if (dish != null && dish.dirty == true)
            {
                if (PlayerController.Instance.isButtonPressed == true)
                {
                    dish.progressValue += 10f * Time.deltaTime;
                }
                if (dish.slider.value == dish.slider.maxValue)
                {
                    dish.GetClean();
                }
            }
        }


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
                    dish.ShowUI();
                }
            }
            else
            {
                if (dish.dirty == false)
                {
                    player.item = dish;
                    dish.HideUI();
                    player.item.transform.SetParent(player.transform);
                    player.item.transform.localPosition = Vector3.up + Vector3.forward;
                    dish.progressValue = 0;
                    dish = null;

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

