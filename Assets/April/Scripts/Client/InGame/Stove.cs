using April;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static UnityEditor.Progress;

public class Stove : InteractionBase
{
    public override bool IsAutoInteractable => true;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.Stove;

    private PlayerController player;
    private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

    private Food foodComponent;

    //public static event Action<Meat> OnMeatCreated;
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
           
                if (foodComponent.CookingState != (int)Meat.MeatState.Burned)
                {
                    foodComponent.progressValue += burningPower * Time.deltaTime;
                }
            
        }
    }

    void StoveInteract()
    {
        // 플레이어가 아이템을 가지고 있다면
        if (player.item != null)
        {
            
            foodComponent = player.item;
           
            // meat에 붙어있는 slider을 켜라
            foodComponent.ShowUI();
            foodComponent.transform.SetParent(this.transform);
            foodComponent.transform.localPosition = Vector3.up;
            foodComponent.gameObject.SetActive(true);
            // 참조를 없애겠다.
            player.item = null;
            Debug.Log("Item Insert To Stove!");
        }
        else if (player.item == null)
        {
            player.item = foodComponent;
            foodComponent = foodComponent.GetComponent<Meat>();
            foodComponent.HideUI();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            foodComponent = null;
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;

        var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
        interactUI.InitActions(interactActionDatas);
    }

    public override void Exit()
    {

    }
}
