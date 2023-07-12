using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Stove : InteractionBase
{
    public override void Interact(PlayerController player)
    {
        if (player.item != null)
        {
            Destroy(player.item);
            player.item = null;
            Debug.Log("Item Insert To Stove!");
        }
    }
}
