using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerFactory : MonoBehaviour
    {
        public static CustomerFactory Instance { get; private set; }

        public Dictionary<int, List<Customer>> waitingCustomersNum = new Dictionary<int, List<Customer>>();
        public int isGroup;
        public Customer customerPrefab;
        public Transform waitingPos;

        public List<CustomerTable> tables = new List<CustomerTable>();
        public List<Customer> waitingCustomers = new List<Customer>();

        public bool allAsigned;
        private float currentTime;
        private float maxTime = 10f;
        public int waitNum;
        //[SerializeField] List<CustomerTable> tables = new List<CustomerTable>();

        private void Awake()
        {
            Instance = this;
            customerPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            SpawnCustomer();
        }
        //private void FindTarget(Customer customer)
        //{
        //    if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
        //    {
        //        foreach (InteractionBase table in tables)
        //        {
        //            var customerTable = table as CustomerTable;
        //            if (customerTable.customerAssigned == true)
        //            {
        //                continue;
        //            }
        //            foreach (CustomerTable_InteractSlot chairPos in customerTable.chairPos)
        //            {
        //                if (chairPos.customerAssigned == true)
        //                {
        //                    continue;
        //                }
        //                customer.myTable = customerTable;
        //                customer.mySeat = chairPos;
        //                customer.mySeat.customerAssigned = true;
        //                customer.MoveToTarget(chairPos.transform.position, () =>
        //                {
        //                    customer.orderImageDisplay.gameObject.SetActive(true);
        //                    transform.LookAt(customer.myTable.transform.position, Vector3.up);
        //                });

        //            }

        //        }
        //    }

        //}
        private void OnDestroy()
        {
            Instance = null;
        }

        public void GetRandomBool()
        {
            isGroup = Random.Range(0, 2);
        }

        public void SpawnCustomer()
        {
            allAsigned = true;
            GetRandomBool();
            for (int i = 0; i <= isGroup; i++)
            {
                Customer customerInstance = Instantiate(customerPrefab, transform.position, Quaternion.identity);
                customerInstance.gameObject.SetActive(true);
                foreach (CustomerTable table in tables)
                {
                    if (!table.customerAssigned)
                    {
                        allAsigned = false;
                    }
                }
                if (isGroup == 1)
                {
                    customerInstance.isGroup = true;
                }
                if (allAsigned == true)
                {
                    waitingCustomers.Add(customerInstance);
                    customerInstance.waiting = true;
                    customerInstance.waitingPos = waitingPos;
                    customerInstance.currentCustomerState = Customer.CustomerState.Waiting;
                }
                //FindTarget(customerInstance);
                customerInstance.exitTarget = this.transform;
                // customerInstance.tables = tables;

                var randomColor = new Color();
                randomColor.r = Random.Range(0, 256) / 255f;
                randomColor.g = Random.Range(0, 256) / 255f;
                randomColor.b = Random.Range(0, 256) / 255f;
                randomColor.a = Random.Range(0, 256) / 255f;

                customerInstance.GraphicColor = randomColor;

            }
            if (allAsigned == true)
            {
                waitingCustomersNum[waitNum] = new List<Customer>(waitingCustomers);
                waitNum++;
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

