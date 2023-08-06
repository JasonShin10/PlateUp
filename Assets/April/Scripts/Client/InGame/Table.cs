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

    float tableHeight;
    float dishHeight;
    float foodHeight;
    private new void Start()
    {
        base.Start();
        tableHeight = GetComponent<Collider>().bounds.size.y;
        
    }

    void TableInteract()
    {
        if (player.item != null)
        {
            if (player.item is Dish)
            {
                if (item != null)
                {
                    Debug.Log("Something is already on table");
                    return;
                }
                else
                {
                    item = player.item;   
                    dishHeight = item.GetComponentInChildren<Collider>().bounds.size.y;
                    item.transform.SetParent(this.transform);
                    item.transform.localPosition = new Vector3(0, tableHeight + dishHeight/2, 0);
                    player.item = null;
                    Debug.Log("Dish Insert To Table!");
                }
            }
            else
            {
                if (item is Dish)
                {
                        foodHeight = player.item.GetComponentInChildren<Collider>().bounds.size.y;
                    switch (player.item)
                    {
                        case Meat:
                            player.item.transform.SetParent(item.transform);                           
                            player.item.transform.localPosition = new Vector3(0,dishHeight + foodHeight/2,0);
                            player.item = null;
                            break;
                        case Chicken:
                            player.item.transform.SetParent(item.transform);
                            player.item.transform.localPosition = new Vector3(0, dishHeight + foodHeight / 2, 0);
                            player.item = null;
                            break;
                    }

                }
                else
                {
                    item = player.item;

                    item.transform.SetParent(this.transform);
                    item.transform.localPosition = new Vector3(0, tableHeight + foodHeight / 2, 0); ;
                    player.item = null;
                    Debug.Log("Food Insert To Table!");
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
