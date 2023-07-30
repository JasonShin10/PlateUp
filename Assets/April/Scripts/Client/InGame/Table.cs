using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : InteractionBase
{
    public override bool IsAutoInteractable => false;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.Table;


    public PlayerController player;
 
   

    private Food foodComponent;

    void TableInteract()
    {
        if (player.item != null)
        {
            foodComponent = player.item;
            
            
            foodComponent.transform.SetParent(this.transform);
            foodComponent.transform.localPosition = Vector3.up;
            foodComponent.gameObject.SetActive(true);
            player.item = null;
            Debug.Log("Item Insert To Table!");
        }
        else if (player.item == null)
        {
            player.item = foodComponent;
            foodComponent = foodComponent.GetComponent<Meat>();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            
            foodComponent = null;
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
