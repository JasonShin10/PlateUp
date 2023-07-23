using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class FoodContainer : InteractionBase
    {
        public override bool IsAutoInteractable => true;

        private PlayerController player;
        public CinemachineVirtualCamera containerCamera;
        public GameObject beefPrefab;
        public GameObject chickenPrefab;

        private List<InteractActionData> interactActionDatas = new List<InteractActionData>();


        private void Awake()
        {
            //var actionData = new InteractActionData();
            //actionData.callback += Execute;

            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Meat Interact",
                callback = MeatInteract
            });

            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Chicken Interact",
                callback = ChickenInteract
            });

            containerCamera.gameObject.SetActive(false);
        }


        void MeatInteract()

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

        void ChickenInteract()
        {
            if (player.item == null)
            {
                var newFoodItem = Instantiate(chickenPrefab);
                newFoodItem.transform.localScale = chickenPrefab.transform.localScale;
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


            containerCamera.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            containerCamera.gameObject.SetActive(false);
        }
    }
}