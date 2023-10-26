using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEditorInternal;
using UnityEngine;

namespace April
{
    [Serializable]
    public class CharacterJobTaskBase
    {
        public Vector3 destination;
        public Action executeAction;
    }




    public class Character_Waitress : CharacterBase
    {
        public static List<Character_Waitress> SpawnedWaitressList = new List<Character_Waitress>();
        public override CharacterType CharacterType => CharacterType.Waitress;
        public bool HasJobTask => currentJob != null;

        public Transform waitingPosition;
        public DishTable dishTable;
        public WaitressTable waitressTable;
        public TrashCan trashCan;

        public int foodState;

        private Customer currentTargetCustomer;
        private Queue<CharacterJobTaskBase> jobTasks = new Queue<CharacterJobTaskBase>();
        [ShowInInspector]


        private CharacterJobTaskBase currentJob;

        public event Action OnInsertedJob;
        [Title("Visualization")]
        public VisualizationCharacter visualization;

        protected override void Awake()
        {
            base.Awake();
            SpawnedWaitressList.Add(this);
        }

        public override void Start()
        {
            base.Start();

            
            waitressTable.OnFoodArrived += HandleFoodArrived;
            GetOrder();
        }

        protected override void Update()
        {
            base.Update();
            if (isMoving == true)
            {
                visualization.SetMovement(0.5f);
            }
            else
            {
                visualization.SetMovement(0);
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            SpawnedWaitressList.Remove(this);
        }

        public void Setup(Transform spawnP, DishTable dishT, WaitressTable waitressT, TrashCan trashC)
        {
            waitingPosition = spawnP;
            dishTable = dishT;
            waitressTable = waitressT;
            trashCan = trashC;            
        }

        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            AddJobTask(currentTargetCustomer.transform.position, OnDestinationCustomer);
        }

        public void HandleFoodArrived()
        {
            Debug.Log("HandleFoodArrived");
            AddJobTask(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
        }

        public void FindWaitingFoodCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;

            foreach (Customer customer in IngameWaiterSystem.Instance.waitingFoodCustomerList)
            {
                if (customer.orderedMenuType == (int)food.MenuType && customer.orderedMenuStateType == food.CookingState)
                {
                    if ((int)customer.state < minStateValue)
                    {
                        minStateValue = (int)customer.state;
                    }
                    if (customer.patienceSlider.value < minPatienceValue)
                    {
                        minPatienceValue = customer.patienceSlider.value;
                        minPatiecneCustomer = customer;
                    }
                }
            }

            if (minPatiecneCustomer)
            {         
                ReceiveCustomerOrder(minPatiecneCustomer);
            }
            else
            {
                AddJobTask(trashCan.transform.position, OnWaitressArrivedTrashCan);
            }
        }

        private void GetOrder()
        {
            if (IngameWaiterSystem.Instance.waitingOrderCustomerList.Count > 0)
            {
                ReceiveCustomerOrder(IngameWaiterSystem.Instance.waitingOrderCustomerList[0]);
            }
        }

        private void OnDestinationCustomer()
        {
            if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(this);
                currentTargetCustomer = null;

                if (IngameWaiterSystem.Instance.waitingOrderCustomerList.Count > 0)
                {
                    ReceiveCustomerOrder(IngameWaiterSystem.Instance.waitingOrderCustomerList[0]);
                }
                else
                {
                    this.SetDestination(waitingPosition.position);
                }
            }
        }


        private void OnWaitressArrivedTable()
        {
            waitressTable.WaitressInteract(this);
            if (dish)
            {
                food = dish.ContainedFoodItems[0];
                foodState = food.CookingState;
                FindWaitingFoodCustomer();
            }
        }

        public void OnWaitressArrivedTrashCan()
        {
            trashCan.Interact(this);
            AddJobTask(dishTable.transform.position, OnWaitressArrivedDishTable);
        }

        public void OnWaitressArrivedDishTable()
        {
            dishTable.Interact(this);
            this.SetDestination(waitingPosition.transform.position);
        }

        public void AddJobTask(Vector3 destination, Action executeAction)
        {
            var newJob = new CharacterJobTaskBase()
            {
                destination = destination,
                executeAction = executeAction
            };
            jobTasks.Enqueue(newJob);

            StartCoroutine(DelayedNewJobTaskReceiveNotify());
            IEnumerator DelayedNewJobTaskReceiveNotify()
            {
                yield return new WaitForEndOfFrame();
                OnReceivedNewJob();
            }
        }
        private void OnReceivedNewJob()
        {
            if (currentJob != null)
                return;

            ExecuteJob();
        }

        public void ExecuteJob()
        {
            if (jobTasks.Count <= 0)
                return;

            var nextJob = jobTasks.Dequeue();
            currentJob = nextJob;
            currentJob.executeAction += ClearJob;
            this.SetDestination(nextJob.destination, nextJob.executeAction);
        }

        private void ClearJob()
        {
            currentJob = null;

            StartCoroutine(DelayedGetNextJob());
            IEnumerator DelayedGetNextJob()
            {
                yield return new WaitForEndOfFrame();
                ExecuteJob();
            }
        }

        public void ForceExecuteJob(CharacterJobTaskBase job)
        {
            currentJob = job;
            this.SetDestination(job.destination, job.executeAction);
        }
    }
}

