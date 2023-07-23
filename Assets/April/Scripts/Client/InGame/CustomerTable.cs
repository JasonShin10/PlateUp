using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTable : InteractionBase
{

    private PlayerController player;
    private GameObject newFoodItem;
    private Meat meatComponent;

    void CustomerTableInteract()
    {
        if (player.item != null)
        {
            newFoodItem = player.item;

            meatComponent = newFoodItem.GetComponent<Meat>();
            newFoodItem.transform.SetParent(this.transform);
            newFoodItem.transform.localPosition = Vector3.up;
            newFoodItem.gameObject.SetActive(true);
            player.item = null;
            Debug.Log("Item Insert To CustomerTable!");
        }
        else if (player.item == null)
        {
            player.item = newFoodItem;
            meatComponent = newFoodItem.GetComponent<Meat>();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            newFoodItem = null;
        }
    }
 
    public override void Interact(PlayerController player)
    {
        this.player = player;
        CustomerTableInteract();
    }

    public override void Exit()
    {

    }
}