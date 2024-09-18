
using System.Collections.Generic;

using UnityEngine;

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
        public GameObject[] fireQuads;
        public Renderer[] fires;
        public Transform spawnPoint;
        public IngameUpgradeSystem ingameUpgradeSystem;
        [SerializeField] private float offSet = 2.2f;

        private int idx = 0;
        protected override void Awake()
        {
            base.Awake();
            
            FireColorChange(0);
            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Stove Action",
                callback = StoveInteract
            });
            if (ingameUpgradeSystem == null)
            {
                Debug.LogError("IngameUpgradeSystem is not assigned!");
                return; // 혹은 다른 대체 처리
            }
            ingameUpgradeSystem.onUpgradeComplete += OnUpgradeComplete;
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

        private void OnUpgradeComplete()
        {
            if (idx < 3)
            {
                idx++;
            }
           
            FireColorChange(idx);
        }
        public void FireColorChange(int idx)
        {
            foreach (Renderer fire in fires)
            {
                fire.material.SetColor("_FireColor", RuntimeStoveData.fireColor[idx]);
            }
        }
        
        void FireControl(bool isOn)
        {
            if (isOn == true)
            {
                foreach (GameObject quad in fireQuads)
                {
                    quad.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject quad in fireQuads)
                {
                    quad.SetActive(false);
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

                FireControl(true);
                
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
                FireControl(false);
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

