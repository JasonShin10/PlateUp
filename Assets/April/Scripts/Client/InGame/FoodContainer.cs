using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class FoodContainer : InteractionBase
    {
        public override bool IsAutoInteractable => true;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.Container;

        private PlayerController player;
        public CinemachineVirtualCamera containerCamera;
        public Beef beefPrefab;
        public ThickBeef thickBeefPrefab;
        public Food beef;
        private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

        protected override void Awake()
        {
            base.Awake();

            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Beef Interact",
                callback = BeefInteract
            });

            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "ThickBeef Interact",
                callback = ThickBeefInteract
            });

            containerCamera.gameObject.SetActive(false);
        }

        public void SpawnBeefToPlayer(Food beef)
        {
            var newFoodItem = Instantiate(beef);
            newFoodItem.transform.localScale = beef.transform.localScale;
            newFoodItem.transform.SetParent(player.transform);
            newFoodItem.transform.position = player.spawnPos.position;
            newFoodItem.gameObject.SetActive(true);
            player.item = newFoodItem;
        }

        void BeefInteract()
        {
            if (player.item == null)
            {
                SpawnBeefToPlayer(beefPrefab);
            }
        }

        void ThickBeefInteract()
        {
            if (player.item == null)
            {
                SpawnBeefToPlayer(thickBeefPrefab);
            }
        }
        public override void Interact(CharacterBase character)
        {

            this.player = character as PlayerController;

            if (this.player != null)
            {

            var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
            interactUI.InitActions(interactActionDatas);


            containerCamera.gameObject.SetActive(true);
            }
        }

        public override void Exit()
        {
            containerCamera.gameObject.SetActive(false);
        }
    }
}