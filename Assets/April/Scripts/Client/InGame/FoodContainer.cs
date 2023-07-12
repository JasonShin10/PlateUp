using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : InteractionBase
{
    public GameObject foodPrefab;

    private PlayerController player;
    private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

    private void Awake()
    {
        interactActionDatas.Add(new InteractActionData()
        {
            actionName = "Container Interact",
            callback = FoodContainerInteract
        });

        interactActionDatas.Add(new InteractActionData()
        {
            actionName = "Container Destroy",
            callback = FoodContainerDestory
        });
    }

    void FoodContainerInteract()
    {
        if (player.item == null)
        {
            var newFoodItem = Instantiate(foodPrefab, player.transform);
            newFoodItem.transform.localPosition = Vector3.up + Vector3.forward;
            newFoodItem.gameObject.SetActive(true);
            player.item = newFoodItem;
        }
    }

    void FoodContainerDestory()
    {
        Destroy(gameObject);
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        if(player.item == null)
        {
            var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
            interactUI.InitActions(interactActionDatas);
        }
    }
}
