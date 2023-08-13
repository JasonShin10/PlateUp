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
        public ThickBeef ThickBeefPrefab;

        private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

        protected override void Awake()
        {
            base.Awake();
            //var actionData = new InteractActionData();
            //actionData.callback += Execute;

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


        void BeefInteract()
        {
            if (player.item == null)
            {
                var newFoodItem = Instantiate(beefPrefab);
                newFoodItem.transform.localScale = beefPrefab.transform.localScale;
                newFoodItem.transform.SetParent(player.transform);
                newFoodItem.transform.localPosition = Vector3.up + Vector3.forward;
                newFoodItem.gameObject.SetActive(true);
                player.item = newFoodItem;
            }
        }

        void ThickBeefInteract()
        {
            if (player.item == null)
            {
                var newFoodItem = Instantiate(ThickBeefPrefab);
                newFoodItem.transform.localScale = ThickBeefPrefab.transform.localScale;
                newFoodItem.transform.SetParent(player.transform);
                newFoodItem.transform.localPosition = Vector3.up + Vector3.forward;
                newFoodItem.gameObject.SetActive(true);
                player.item = newFoodItem;
            }
        }
        public override void Interact(PlayerController player)
        {
            this.player = player;

            var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
            interactUI.InitActions(interactActionDatas);

            player.visualization.SetInteractionFoodContainer(true);
            containerCamera.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            containerCamera.gameObject.SetActive(false);
        }
    }
}