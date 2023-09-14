using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerFactorySystem : MonoBehaviour
    {
        public static IngameCustomerFactorySystem Instance { get; private set; }

        public int isGroup;
        public Customer customerPrefab;
        public Transform spawnPoint;
        [MinMaxSlider(2, 10, true)]
        public Vector2Int npcGroupSpawnRnage = new Vector2Int(2, 10);
        public List<CustomerTable> tables = new List<CustomerTable>();

        public Character_Waitress waitress; 

        private float currentTime;
        private float maxTime = 10f;

        private void Awake()
        {
            Instance = this;
            customerPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            SpawnCustomer();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [Button("Spawn Customer")]
        public void SpawnCustomer()
        {
            if (IngameCustomerWaitingSystem.Instance.IsFullWaitingSlot)
                return;

            int rand = Random.Range(0, 2);
            bool isGroupSpawn = rand % 2 == 0;
            //bool isGroupSpawn = false;
            int spawnCount = 1;
            if (isGroupSpawn)
            {
                spawnCount = Random.Range(npcGroupSpawnRnage.x, npcGroupSpawnRnage.y + 1);
            }

            if (spawnCount > IngameCustomerWaitingSystem.Instance.EmptyWaitingSlotCount)
                return;

            if (IngameCustomerWaitingSystem.Instance.TryGetEmptySlots(spawnCount, out int groupId, out var waitingSlots))
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    Customer customerInstance = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                    customerInstance.gameObject.SetActive(true);
                    customerInstance.isGroup = isGroupSpawn;

                    customerInstance.waitingPos = waitingSlots[i].transform;

                    customerInstance.groupID = groupId;
                    customerInstance.SetCustomerState(CustomerState.Waiting);
                    
                    customerInstance.exitTarget = this.transform;
                    //waitress.RegisterCustomer(customerInstance);
                    waitingSlots[i].customer = customerInstance;
                    
                    var randomColor = new Color();
                    randomColor.r = Random.Range(0, 256) / 255f;
                    randomColor.g = Random.Range(0, 256) / 255f;
                    randomColor.b = Random.Range(0, 256) / 255f;
                    randomColor.a = Random.Range(0, 256) / 255f;

                    //customerInstance.GraphicColor = randomColor;
                }

                IngameCustomerWaitingSystem.Instance.CheckWaitingCustomerPossibleEnter();
            }
        }

        void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxTime)
            {
                SpawnCustomer();
                currentTime = 0;
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            Destroy(customer.gameObject);
        }
    }
}

