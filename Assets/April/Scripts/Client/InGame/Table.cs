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
            Destroy(player.item);
            player.item = null;
            newFoodItem = Instantiate(foodPrefab);
            meatComponent = newFoodItem.GetComponent<Meat>();
            newFoodItem.transform.SetParent(this.transform);
            newFoodItem.transform.localPosition = Vector3.up;
            newFoodItem.gameObject.SetActive(true);
            Debug.Log("Item Insert To Stove!");
        }
        else if (player.item == null)
        {
            player.item = Instantiate(foodPrefab);
            meatComponent = newFoodItem.GetComponent<Meat>();
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
            Destroy(newFoodItem);
            newFoodItem = null;
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
    }

    private void OnEnable()
    {
        //InputManager.Singleton.InputMaster.Act.InteractionShortcut.performed += OnInteractionShortcut;
    }

    private void OnDisable()
    {
        //InputManager.Singleton.InputMaster.Act.InteractionShortcut.performed -= OnInteractionShortcut;
    }

    private void OnInteractionShortcut(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        TableInteract();
    }
}
