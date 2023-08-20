using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoContainer : InteractionBase
{
    public override bool IsAutoInteractable => false;
    private PlayerController player;
    public Cabbage cabbage;

    public override InteractionObjectType InterationObjectType => InteractionObjectType.CabbageContainer;
    void TomatoContainerInteract()
    {
        if (player.item is Cabbage)
        {
            Destroy(player.item.gameObject);
            player.item = null;
        }
        else if (player.item == null)
        {
            player.item = cabbage;
            cabbage.transform.SetParent(player.transform);
            cabbage.transform.localPosition = Vector3.up + Vector3.forward;
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        TomatoContainerInteract();
    }

    public override void Exit()
    {

    }
}
