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

    private PlayerController player;
    private List<InteractActionData> interactActionDatas = new List<InteractActionData>();
    private GameObject newFoodItem;
    private Meat meatComponent;

    //public static event Action<Meat> OnMeatCreated;
    private void Awake()
    {
        interactActionDatas.Add(new InteractActionData()
        {
            actionName = "Stove Action",
            callback = StoveInteract
        });
    }

    public float burningPower = 3f;


    void Update()
    {
        if (newFoodItem != null)
        {
            if (meatComponent)
            {
                if (meatComponent.State != Meat.MeatState.Burned)
                {
                    meatComponent.progressValue += burningPower * Time.deltaTime;
                }
            }
        }
    }

    void StoveInteract()
    {
        // �÷��̾ �������� ������ �ִٸ�
        if (player.item != null)
        {
            
            newFoodItem = player.item;
            meatComponent = newFoodItem.GetComponent<Meat>();
            // meat�� �پ��ִ� slider�� �Ѷ�
            meatComponent.ShowUI();
            newFoodItem.transform.SetParent(this.transform);
            newFoodItem.transform.localPosition = Vector3.up;
            newFoodItem.gameObject.SetActive(true);
            // ������ ���ְڴ�.
            player.item = null;
            Debug.Log("Item Insert To Stove!");
        }
        else if (player.item == null)
        {
            player.item = newFoodItem;
            meatComponent = newFoodItem.GetComponent<Meat>();
            meatComponent.HideUI();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            newFoodItem = null;
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
