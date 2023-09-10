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
    public class CharacterJobTaskData
    {
        public Vector3 destination;
        public Action action;
    }

    public class Character_Waitress : CharacterBase
    {
        public static List<Character_Waitress> SpawnedWaitressList = new List<Character_Waitress>();
        public override CharacterType CharacterType => CharacterType.Waitress;
        public bool HasJobTask => currentJob != null;

        public Transform waitingPosition;
        public DishTable dishTable;
        public WaitressTable waitressTable;
        public Dish dish;
        public Food food;
        public TrashCan trashCan;
        public int foodState;

        private Customer currentTargetCustomer;
        private Queue<CharacterJobTaskData> jobTasks = new Queue<CharacterJobTaskData>();
        private CharacterJobTaskData currentJob;

        public event Action<CharacterJobTaskData> OnInsertedJob;

        protected override void Awake()
        {
            base.Awake();
            SpawnedWaitressList.Add(this);
        }

        public override void Start()
        {
            base.Start();

            OnInsertedJob += OnReceivedNewJob;
            waitressTable.OnFoodArrived += HandleFoodArrived;
        }

        private void OnReceivedNewJob(CharacterJobTaskData jobData)
        {
            if (currentJob != null)
                return;

            ExecuteJob();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SpawnedWaitressList.Remove(this);
        }

        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            AddJobTask(currentTargetCustomer.transform.position, OnDestinationCustomer);

            //this.SetDestination(currentTargetCustomer.transform.position);
            //StartCoroutine(DelayedRegistDestinationCallback());
            //IEnumerator DelayedRegistDestinationCallback()
            //{
            //    yield return new WaitForEndOfFrame();

            //    OnDestination += OnDestinationCustomer;
            //}
        }

        public void HandleFoodArrived()
        {
            AddJobTask(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
            //this.SetDestination(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
        }

        public void FindWaitingFoodCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;

            foreach (Customer customer in IngameWaiterSystem.Instance.waitingFoodCustomerList)
            {
                if (customer.orderedMenuType == food.MenuType && customer.orderedMenuStateType == food.CookingState)
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

                //this.SetDestination(trashCan.transform.position);
                //StartCoroutine(DelayedRegistDestinationCallback());
                //IEnumerator DelayedRegistDestinationCallback()
                //{
                //    yield return new WaitForEndOfFrame();

                //    OnDestination += OnWaitressArrivedTrashCan;
                //}
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
                else if (IngameWaiterSystem.Instance.waitingFoodCustomerList.Count > 0 && waitressTable.HasFood)
                {
                    AddJobTask(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
                    //this.SetDestination(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
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

            //this.SetDestination(dishTable.transform.position);
            //StartCoroutine(DelayedRegistDestinationCallback());
            //IEnumerator DelayedRegistDestinationCallback()
            //{
            //    yield return new WaitForEndOfFrame();

            //    OnDestination += OnWaitressArrivedDishTable;
            //}
        }

        public void OnWaitressArrivedDishTable()
        {
            dishTable.Interact(this);
            this.SetDestination(waitingPosition.transform.position);
        }

        public void AddJobTask(Vector3 destination, Action executeAction)
        {
            var newJob = new CharacterJobTaskData()
            {
                destination = destination,
                action = executeAction
            };
            jobTasks.Enqueue(newJob);

            Debug.Log($"Job Task Count :{jobTasks.Count}");

            StartCoroutine(DelayedNewJobTaskReceiveNotify());
            IEnumerator DelayedNewJobTaskReceiveNotify()
            {
                yield return new WaitForEndOfFrame();

                OnInsertedJob?.Invoke(newJob);
            }
        }

        public void ExecuteJob()
        {
            if (jobTasks.Count <= 0)
                return;

            var nextJob = jobTasks.Dequeue();
            currentJob = nextJob;
            currentJob.action += ClearJob;
            this.SetDestination(nextJob.destination, nextJob.action);
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

        public void ForceExecuteJob(CharacterJobTaskData job)
        {
            currentJob = job;
            this.SetDestination(job.destination, job.action);
        }

    }
}

