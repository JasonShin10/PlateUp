using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : InteractionBase
{
    public GameObject beefPrefab;
    public GameObject chickenPrefab;
    private PlayerController player;
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
            actionName = "Vegetable Interact",
            callback = VegetableInteract
        });
    }

<<<<<<< HEAD
    void MeatInteract()
=======
    void Execute() { }


    void FoodContainerInteract()
>>>>>>> 9630c032f13bdd3f1c88a479d5e0c0278c5ec80d
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

    void VegetableInteract()
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
    }
}
