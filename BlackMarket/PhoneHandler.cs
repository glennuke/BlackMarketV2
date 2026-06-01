using System.Collections.Generic;
using UnityEngine;

namespace BlackMarketV2
{
    public class CustomNumber
    {
        public string number;
        public float timer;
        public float price;
        public bool isOrdered;
        public List<GameObject> itemsToOrder;
    }

    public static class PhoneHandler
    {
        public static List<CustomNumber> CustomNumbers = new List<CustomNumber>();
        private static bool isInitialized = false;
        private static CustomPhoneOrder CustomPhoneOrderApartment;
        private static CustomPhoneOrder CustomPhoneOrderParents;

        private static void Init()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.name == "KeypadPhone1")
                {
                    CustomPhoneOrderParents = obj.AddComponent<CustomPhoneOrder>();
                }

                if (obj.name == "KeypadPhone2")
                {
                    CustomPhoneOrderApartment = obj.AddComponent<CustomPhoneOrder>();
                }

                break;
            }
            isInitialized = true;
        }

        public static void AddOrder(string phoneNumber, List<GameObject> itemsToOrder, float price)
        {
            if (!isInitialized) Init();
            CustomNumbers.Add(new CustomNumber { number = phoneNumber, itemsToOrder = itemsToOrder, price = price });
        }
    }

    public class CustomPhoneOrder : MonoBehaviour
    {
        PlayMakerFSM CallingFSM;
        CustomNumber customNumberCurrentlyCalling;
        bool isCallingCustom;

        void Start()
        {
            CallingFSM = GetComponents<PlayMakerFSM>()[1];
        }

        void Update()
        {
            foreach (CustomNumber customNumber in PhoneHandler.CustomNumbers)
            {
                if (customNumber.isOrdered)
                {
                    if (customNumber.timer > 0)
                    {
                        customNumber.timer -= Time.deltaTime;
                    }
                    else
                    {
                        foreach (GameObject item in customNumber.itemsToOrder)
                        {
                            GameObject spawnedItem = GameObject.Instantiate(item);
                            spawnedItem.transform.position = new Vector3(-1711.802f, 3.518661f, 924.8834f);
                        }
                        PhoneHandler.CustomNumbers.Remove(customNumber);
                    }
                }
            }

            if (CallingFSM.ActiveStateName == "Find number")
            {
                foreach (CustomNumber customNumber in PhoneHandler.CustomNumbers)
                {
                    if (customNumber.number == CallingFSM.FsmVariables.GetFsmString("Number").Value)
                    {
                        CallingFSM.SendEvent("CALL");
                        customNumberCurrentlyCalling = customNumber;
                        isCallingCustom = true;
                        break;
                    }
                }
            }

            if (isCallingCustom)
            {
                if (CallingFSM.ActiveStateName == "Hangup")
                {
                    customNumberCurrentlyCalling.isOrdered = true;
                    customNumberCurrentlyCalling.timer = Random.Range(600f, 2300f);
                    isCallingCustom = false;
                }
            }
        }
    }
}
