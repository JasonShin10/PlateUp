using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerFactory : MonoBehaviour
    {
        public static CustomerFactory Instance { get; private set; }


        public int limitCount = 1;
        public Customer customerPrefab;

        [SerializeField] private float spawnTime = 3f;

        private float currentTime;
        private List<Customer> createdCustomers = new List<Customer>();

        //[SerializeField] List<CustomerTable> tables = new List<CustomerTable>();

        private void Awake()
        {
            Instance = this;
            customerPrefab.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        void Update()
        {
            if (createdCustomers.Count < limitCount)
            {
                currentTime += Time.deltaTime;
                if (currentTime > spawnTime)
                {
                    Customer customerInstance = Instantiate(customerPrefab, transform.position, Quaternion.identity);
                    customerInstance.gameObject.SetActive(true);
                    customerInstance.exitTarget = this.transform;
                    // customerInstance.tables = tables;

                    var randomColor = new Color();
                    randomColor.r = Random.Range(0, 256) / 255f;
                    randomColor.g = Random.Range(0, 256) / 255f;
                    randomColor.b = Random.Range(0, 256) / 255f;
                    randomColor.a = Random.Range(0, 256) / 255f;
                    customerInstance.GraphicColor = randomColor;

                    createdCustomers.Add(customerInstance);
                    currentTime = 0;
                }
            }
        }
    }
}

