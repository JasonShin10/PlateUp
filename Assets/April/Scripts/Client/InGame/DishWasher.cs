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
        public ParticleController particleController;
        public Transform dirtyDishPivot;
        public Transform cleanDishPivot;


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
                    dish.transform.position = cleanDishPivot.position;
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
                    player.item.transform.position = dirtyDishPivot.position;
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
                    Exit();
                }
            }
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            //this.player.visualization.SetInteractionCook(true);

            DishWasherInteract();
        }

        public override void Exit()
        {
            //this.player.visualization.SetInteractionCook(false);
        }
    }


}

