using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : InteractionBase
{
    public GameObject foodPrefab;
    private PlayerController player;

    private GameObject newFoodItem;
    private Meat meatComponent;


    void TableInteract()
    {
        if (player.item != null)
        {
            newFoodItem = player.item;
            
            meatComponent = newFoodItem.GetComponent<Meat>();
            newFoodItem.transform.SetParent(this.transform);
            newFoodItem.transform.localPosition = Vector3.up;
            newFoodItem.gameObject.SetActive(true);
            player.item = null;
            Debug.Log("Item Insert To Table!");
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
    }

    private void OnEnable()
    {
        InputManager.Singleton.InputMaster.PlayerControl.Interact.performed += OnInteractionShortcut;
    }

    private void OnDisable()
    {
        InputManager.Singleton.InputMaster.PlayerControl.Interact.performed -= OnInteractionShortcut;
    }

    private void OnInteractionShortcut(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        TableInteract();
    }

    public override void Exit()
    {

    }
}
