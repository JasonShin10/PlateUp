using April;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Table : InteractionBase
{
    public override bool IsAutoInteractable => false;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.Table;

    public PlayerController player;

    private InteractionItem item;

    // 인터페이스로 옮겨볼까?
    public float offset = 0.3f;
    private new void Start()
    {
        base.Start();
    }

    void TableInteract()
    {
        if (player.item != null)
        {
            if (player.item is Dish)
            {
                if (item is Food)
                {
                    var dish = player.item as Dish;
                    var food = item as Food;
                    dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                    item = null;
                    Debug.Log("Something is already on table");
                    return;
                }
                else
                {
                    item = player.item;
                    var dish = item as Dish;
                    dish.transform.SetParent(this.transform);
                    dish.transform.localPosition = new Vector3(0, offset, 0);
                    player.item = null;
                    Debug.Log("Dish Insert To Table!");
                }
            }
            else
            {
                if (item is Dish)
                {
                    var dish = item as Dish;
                    var food = player.item as Food;
                    dish.AddItem(food, new Vector3(0, offset +food.offsetOnDish, 0));
                    player.item = null;
                }
                else
                {
                    if (item == null)
                    {
                    item = player.item;
                    item.transform.SetParent(this.transform);
                    item.transform.localPosition = new Vector3(0, offset, 0);
                    player.item = null;
                    Debug.Log("Food Insert To Table!");
                    }
                    
                }

            }
        }
        else
        {
            if (item != null)
            {
                player.item = item;
                item.transform.SetParent(player.transform);
                item.transform.localPosition = Vector3.up + Vector3.forward;
                item = null;
            }
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        TableInteract();
    }

    public override void Exit()
    {

    }
}
