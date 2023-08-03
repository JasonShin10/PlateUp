using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : InteractionBase
{
    public override bool IsAutoInteractable => false;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.Table;

    public PlayerController player;

    private InteractionItem item;

    float tableHeight;
    float itemHalfHeight;

    private new void Start()
    {
        base.Start();
        tableHeight = this.GetComponent<Collider>().bounds.size.y / 6;
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
                    itemHalfHeight = item.GetComponent<Collider>().bounds.size.y / 2f;
                    Vector3 itemPositionAboveTable = new Vector3(0, tableHeight + itemHalfHeight, 0);
                    item.transform.SetParent(this.transform);
                    item.transform.localPosition = itemPositionAboveTable;
                    item.gameObject.SetActive(true);
                    player.item = null;
                    Debug.Log("Dish Insert To Table!");
                }
            }
            else
            {
                if (item is Dish)
                {
                    switch (player.item)
                    {
                        case Meat :
                            player.item.transform.SetParent(item.transform);  
                            player.item.transform.localPosition = item.transform.localPosition;// world랑 local차이점 찾아서 정리
                            player.item = null;
                            break;
                        case Chicken :
                            player.item.transform.SetParent(item.transform);
                            player.item.transform.localPosition = item.transform.localPosition;// world랑 local차이점 찾아서 정리
                            player.item = null;
                            break;
                    }

                }
                else
                {
                    item = player.item;
                    itemHalfHeight = item.GetComponent<Collider>().bounds.size.y / 2f;
                    Vector3 itemPositionAboveTable = new Vector3(0, tableHeight + itemHalfHeight, 0);
                    item.transform.SetParent(this.transform);
                    item.transform.localPosition = itemPositionAboveTable;
                    item.gameObject.SetActive(true);
                    player.item = null;
                    Debug.Log("Food Insert To Table!");
                }

            }
        }
        else
        {
            player.item = item;
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            item = null;
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
