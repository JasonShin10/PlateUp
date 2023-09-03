using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace April
{
    public class CabbageContainer : InteractionBase
    {
        //public override bool IsAutoInteractable { get { return true; } }
        public override bool IsAutoInteractable => false;
        private PlayerController player;
        public Cabbage cabbage;

        public override InteractionObjectType InterationObjectType => InteractionObjectType.CabbageContainer;
        void CabbageContainerInteract()
        {
            if (player.item is Cabbage)
            {
                Destroy(player.item.gameObject);
                player.item = null;
            }
            else if(player.item == null)
            {
            SpawnCabbageToPlayer();
            }
        }
        public void SpawnCabbageToPlayer()
        {
            var newFoodItem = Instantiate(cabbage);
            newFoodItem.transform.localScale = cabbage.transform.localScale;
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
                CabbageContainerInteract();

            }
        }

        public override void Exit()
        {

        }
    }
}

