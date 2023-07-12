using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Stove : InteractionBase
{

    private PlayerController player;
    private List<InteractActionData> interactActionDatas = new List<InteractActionData>();

    private void Awake()
    {
        interactActionDatas.Add(new InteractActionData()
        {
            actionName = "Stove Action",
            callback = StoveInteract
        });
    }

    void StoveInteract()
    {
        if (player.item != null)
        { 
            Destroy(player.item);
            player.item = null;
            Debug.Log("Item Insert To Stove!");
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        if (player.item != null)
        {
            var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
            interactUI.InitActions(interactActionDatas);
        }
    }
}
