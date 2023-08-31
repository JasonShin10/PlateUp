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
            player.item = cabbage;
            cabbage.transform.SetParent(player.transform);
            cabbage.transform.localPosition = Vector3.up + Vector3.forward;
            }
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

