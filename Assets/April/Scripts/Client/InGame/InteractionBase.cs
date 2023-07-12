using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class InteractionBase : MonoBehaviour
    {
        public GameObject foodPrefab;

        public virtual GameObject Interact(Transform playerTransform)
        {
            Vector3 foodPosition = playerTransform.position + Vector3.up * 2;
            GameObject item = Instantiate(foodPrefab, foodPosition, Quaternion.identity);  
            item.transform.SetParent(playerTransform); 
            return item;
        }
    }


}

