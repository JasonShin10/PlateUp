
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace April
{
    public class IngameUpgradeSystem : MonoBehaviour
    {
        public static IngameUpgradeSystem Instance { get; private set; }

        [field: SerializeField] public StoveData RuntimeStoveData { get; set; }

        public Action buttonFunction1;
        public Action buttonFunction2;

        public List<Action> buttonFunctions;



        [Title("spawnWaitress")]
        public Character_Waitress waitress;
        public Transform spawnPos;

        public DishTable dishTable;
        public TrashCan trashCan;
        public WaitressTable waitressTable;

        [Title("Stove")]
        public List<Stove> stoves;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            //buttonFunctions = new List<Action> { SpawnWaitress, UpgradeStove, IncreasePatience, IncreaseSpeed };
            buttonFunctions = new List<Action> { UpgradeStove, UpgradeStove, UpgradeStove, UpgradeStove };
            //buttonFunctions = new List<Action> { SpawnWaitress, SpawnWaitress, SpawnWaitress, SpawnWaitress };

            RuntimeStoveData.Initialize();
        }

        public void PickRandomOption(out string text1, out string text2)
        {
            int random1 = UnityEngine.Random.Range(0, buttonFunctions.Count);
            buttonFunction1 = buttonFunctions[random1];
            text1 = GetDescription(buttonFunctions[random1]);
            buttonFunctions.Remove(buttonFunctions[random1]);
            int random2 = UnityEngine.Random.Range(0, buttonFunctions.Count);
            buttonFunction2 = buttonFunctions[random2];
            text2 = GetDescription(buttonFunctions[random2]);
            buttonFunctions.Add(buttonFunction1);
        }

        private void SpawnWaitress()
        {
            Debug.Log("SpawnWaitress");
            var newWaitress = Instantiate(waitress);
            newWaitress.Setup(spawnPos, dishTable, waitressTable, trashCan);
            newWaitress.transform.position = spawnPos.position;
            Debug.Log("Waitress created at: " + newWaitress.transform.position);
            buttonFunctions.Remove(this.SpawnWaitress);
        }

        private void UpgradeStove()
        {
            Debug.Log("UpgradeStove");
            RuntimeStoveData.BurningPower *= 2;
            //foreach (var stove in stoves)
            //{
            //    stove.burningPower *= 2;
            //}
        }

        private void IncreasePatience()
        {
            Debug.Log("IncreasePatience");
        }

        private void IncreaseSpeed()
        {
            Debug.Log("IncreaseSpeed");

        }
        private string GetDescription(Action action)
        {
            if (action == SpawnWaitress)
                return "Spawn Waitress";
            if (action == UpgradeStove)
                return "Upgrade Stove";
            if (action == IncreasePatience)
                return "Increase Patience";
            if (action == IncreaseSpeed)
                return "Increase Speed";



            return "";
        }


    }
}