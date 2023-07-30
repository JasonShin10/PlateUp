using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishTable : InteractionBase
{
    public override bool IsAutoInteractable => false;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.DishTable;

    private PlayerController player;



    public Dish dish;

    void DishTableInteract()
    {
      if (player.dish == null)
        {
            var newDish = Instantiate(dish);
            newDish.transform.localScale = dish.transform.localScale;
            newDish.transform.SetParent(player.transform);
            newDish.transform.localPosition = Vector3.up + Vector3.forward;
            newDish.gameObject.SetActive(true);
            player.dish = newDish;

        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        DishTableInteract();
    }

    public override void Exit()
    {

    }
}
