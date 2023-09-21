using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static UnityEditor.Progress;

namespace April
{
    public class Stove : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.Stove;

        private PlayerController player;
        private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

        private InteractionItem item;
        private Food foodComponent;
        [SerializeField] private float offSet = 2.2f;
        //public static event Action<Beef> OnBeefCreated;
        protected override void Awake()
        {
            base.Awake();
            interactActionDatas.Add(new InteractActionData()
            {
                actionName = "Stove Action",
                callback = StoveInteract
            });

        }

        public float burningPower = 3f;


        void Update()
        {
            if (foodComponent != null)
            {
                if (foodComponent.CookingState != (int)Beef.BeefState.Burned)
                {
                    foodComponent.progressValue += burningPower * Time.deltaTime;
                }
            }
        }

        //private void PassItem(Transform itemTransform)
        //{
        //    itemTransform.SetParent(this.transform);
        //    Collider collider = GetComponent<Collider>();
        //    float height = collider.bounds.size.y;
        //    itemTransform.position = itemTransform.position + new Vector3(0, height, 0);
        //}


        void StoveInteract()
        {
            // 플레이어가 아이템을 가지고 있다면
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


                // Beef에 붙어있는 slider을 켜라

                foodComponent.ShowUI();
                foodComponent.transform.SetParent(this.transform);
                foodComponent.transform.localPosition = new Vector3(0, offSet,0);
                
                // 참조를 없애겠다.
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

