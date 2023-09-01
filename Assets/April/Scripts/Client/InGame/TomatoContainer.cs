
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace April
{

    public class TomatoContainer : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        private PlayerController player;
        public Tomato tomato;

        public override InteractionObjectType InterationObjectType => InteractionObjectType.CabbageContainer;
        void TomatoContainerInteract()
        {
            if (player.item is Cabbage)
            {
                Destroy(player.item.gameObject);
                player.item = null;
            }
            else if (player.item == null)
            {
                SpawnTomatoToPlayer();
            }
        }

        public void SpawnTomatoToPlayer()
        {
            var newFoodItem = Instantiate(tomato);
            newFoodItem.transform.localScale = tomato.transform.localScale;
            newFoodItem.transform.SetParent(player.transform);
            newFoodItem.transform.localPosition = Vector3.up + Vector3.forward;
            newFoodItem.gameObject.SetActive(true);
            player.item = newFoodItem;
        }

        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                TomatoContainerInteract();

            }
        }

        public override void Exit()
        {

        }
    }
}