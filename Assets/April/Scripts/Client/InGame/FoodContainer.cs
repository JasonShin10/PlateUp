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
        public void SpawnBeefToPlayer(Food beef)
        {
            var newBeef = Instantiate(beef);
            newBeef.transform.localScale = beef.transform.localScale;
            newBeef.transform.SetParent(player.transform);
            newBeef.transform.position = player.spawnPos.position;
            newBeef.gameObject.SetActive(true);
            player.item = newBeef;
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