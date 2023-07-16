using April;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Stove : InteractionBase
{
    public GameObject foodPrefab;
    private PlayerController player;
    private List<InteractActionData> interactActionDatas = new List<InteractActionData>();
    private GameObject newFoodItem;
    private Meat meatComponent;
    private void Awake()
    {
        interactActionDatas.Add(new InteractActionData()
        {
            actionName = "Stove Action",
            callback = StoveInteract
        });
    }

    //float currentTime;
    //[SerializeField] float timePerState = 1f;

    public float burningPower = 3f;

    void Update()
    {
        if (newFoodItem != null)
        {
            if (meatComponent)
            {
                if (meatComponent.State != Meat.MeatState.Burned)
                {
                    meatComponent.progressValue += burningPower * Time.deltaTime;
                }
            }


            // 고기가 Burned 상태가 아닌 경우에만 처리
            //if (meatComponent.currentState != Meat.MeatState.Burned)
            //{
            //    currentTime += Time.deltaTime;

            //    if (currentTime > timePerState)
            //    {
            //        meatComponent.currentState++;
            //        Debug.Log(meatComponent.currentState);
            //        currentTime = 0f;
            //    }
            //}
        }
    }

    void StoveInteract()
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

        var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
        interactUI.InitActions(interactActionDatas);


    }

    public override void Exit()
    {

    }
}
