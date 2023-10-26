using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace April
{
    public class Stove : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.Stove;
        
        [field: SerializeField] public StoveData RuntimeStoveData { get; set; }


        private PlayerController player;
        private List<InteractActionData> interactActionDatas = new List<InteractActionData>();
        private InteractionItem item;
        private Food foodComponent;
        public Transform spawnPoint;
        [SerializeField] private float offSet = 2.2f;

        protected override void Awake()
        {
            base.Awake();
            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Stove Action",
                callback = StoveInteract
            });

        }


        void Update()
        {
            if (foodComponent != null)
            {
                if (foodComponent.CookingState != (int)Beef.BeefState.Burned)
                {
                    foodComponent.progressValue += RuntimeStoveData.BurningPower * Time.deltaTime;
                }
            }
        }


        void StoveInteract()
        {

            if (player.item != null)
            {
                this.item = player.item;
                if (player.item is Food)
                {
                    foodComponent = item as Food;
                }
                else
                {
                    return;
                }

                foodComponent.ShowUI();
                foodComponent.transform.SetParent(this.transform);
                foodComponent.transform.position = spawnPoint.position;
                
               
                player.item = null;
               
            }
            else if (player.item == null)
            {
                player.item = foodComponent;
                foodComponent.HideUI();
                player.item.transform.SetParent(player.transform);
                player.item.transform.position = player.spawnPos.position;
                foodComponent = null;
            }
        }

        public override void Interact(CharacterBase character)
        {
            this.player = character as PlayerController;

            if (this.player != null)
            {
                StoveInteract();

            }
        }

        public override void Exit()
        {

        }
    }

}

