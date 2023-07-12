using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : InteractionBase
{
    public GameObject foodPrefab;

    public override void Interact(PlayerController player)
    {
        if (player.item == null)
        {
            var newFoodItem = Instantiate(foodPrefab, player.transform);
            newFoodItem.transform.localPosition = Vector3.up + Vector3.forward;
            newFoodItem.gameObject.SetActive(true);
            player.item = newFoodItem;
        }
    }
}
