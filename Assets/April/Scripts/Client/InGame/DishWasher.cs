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
        public float speed = 10f;

        public void Update()
        {
            if (DishNeedsCleaning())
            {
                UpdateCleaningProgress();
                CompleteCleaningDish();
            }
        }

        private bool DishNeedsCleaning()
        {
            return dish != null && dish.dirty;
        }

        private void UpdateCleaningProgress()
        {
            if (PlayerController.Instance.isButtonPressed &&
                PlayerController.Instance.currentInteractionObject == this)
            {
                dish.progressValue += speed * Time.deltaTime;
            }
        }

        private void CompleteCleaningDish()
        {
            if (dish.slider.value == dish.slider.maxValue)
            {
                dish.GetClean();
                dish.transform.position = cleanDishPivot.position;
            }
        }


        void DishWasherInteract()
        {
            if (player.item != null)
            {
                HandleDirtyDish();
            }
            else
            {
                HandleCleanDish();
            }
        }

        private void HandleDirtyDish()
        {
            dish = player.item as Dish;
            if (dish != null && dish.dirty == true)
            {
                MoveDishToWasher();
                dish.ShowUI();
            }
        }

        private void HandleCleanDish()
        {
            if (dish.dirty == false)
            {
                MoveDishToPlayer();
                ResetDish();
            }
        }

        private void MoveDishToWasher()
        {
            player.item.transform.SetParent(this.transform);
            player.item.transform.position = dirtyDishPivot.position;
            player.item = null;
        }

        private void MoveDishToPlayer()
        {
            player.item = dish;
            dish.HideUI();
            player.item.transform.SetParent(player.transform);
            player.item.transform.position = player.spawnPos.position;
        }

        private void ResetDish()
        {
            dish.progressValue = 0;
            dish = null;
        }
        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                DishWasherInteract();
            }
        }

        public override void Exit()
        {

        }
    }


}

