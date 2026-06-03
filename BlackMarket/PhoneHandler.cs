using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MSCLoader;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlackMarketV2
{
    public class CustomNumber
    {
        public string number;
        public float timer;
        public float price;
        public bool isOrdered;
        public string customSubtitle;
        public Action onOrderAction;
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
            ModConsole.Log("initializing PhoneHander");
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name == "KeypadPhone1")
                {
                    CustomPhoneOrderParents = obj.AddComponent<CustomPhoneOrder>();
                }

                if (obj.name == "KeypadPhone2" && obj.transform.root.name == "HOMENEW")
                {
                    CustomPhoneOrderApartment = obj.AddComponent<CustomPhoneOrder>();
                }
            }
            isInitialized = true;
        }

        public static void AddOrder(string phoneNumber, float price, Action onOrderAction = null, List<GameObject> itemsToOrder = null, string customSubtitle = "")
        {
            if (!isInitialized) Init();
            CustomNumbers.Add(new CustomNumber { number = phoneNumber, itemsToOrder = itemsToOrder, price = price, onOrderAction = onOrderAction, customSubtitle = customSubtitle });
            ModConsole.Log("Added custom phone number " + phoneNumber);
        }
    }

    public class CallbackAction : FsmStateAction
    {
        private Action callback;

        public CallbackAction(Action callback)
        {
            this.callback = callback;
        }

        public override void OnEnter()
        {
            callback?.Invoke();
            Finish();
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

            FsmState findNumberState = CallingFSM.GetState("Find number");

            findNumberState.InsertAction(0, new CallbackAction(() =>
            {
                string number = CallingFSM.FsmVariables.GetFsmString("Number").Value;

                foreach (CustomNumber customNumber in PhoneHandler.CustomNumbers)
                {
                    if (customNumber.number == number)
                    {
                        findNumberState.GetAction<GameObjectCompare>(3).Enabled = false;
                        customNumberCurrentlyCalling = customNumber;
                        isCallingCustom = true;
                        break;
                    }
                }
            }));

            FsmState callState = CallingFSM.GetState("Call");

            callState.InsertAction(4, new CallbackAction(() =>
            {
                if (isCallingCustom)
                {
                    findNumberState.GetAction<GameObjectCompare>(3).Enabled = true;
                    if (!customNumberCurrentlyCalling.isOrdered)
                    {
                        PlayMakerGlobals.Instance.Variables.GetFsmString("GUIsubtitle").Value = customNumberCurrentlyCalling.customSubtitle;
                    }
                }
            }));

            FsmState state2State = CallingFSM.GetState("State 2");

            state2State.InsertAction(2, new CallbackAction(() =>
            {
                if (isCallingCustom)
                {
                    findNumberState.GetAction<GameObjectCompare>(3).Enabled = true;
                    if (!customNumberCurrentlyCalling.isOrdered)
                    {
                        PlayMakerGlobals.Instance.Variables.GetFsmString("GUIsubtitle").Value = customNumberCurrentlyCalling.customSubtitle;
                    }
                }
            }));

            FsmState hangupState = CallingFSM.GetState("Hangup");

            hangupState.InsertAction(0, new CallbackAction(() =>
            {
                if (isCallingCustom)
                {
                    if (!customNumberCurrentlyCalling.isOrdered)
                    {
                        customNumberCurrentlyCalling.onOrderAction?.Invoke();
                        customNumberCurrentlyCalling.isOrdered = true;
                        customNumberCurrentlyCalling.timer = Random.Range(600f, 2300f);
                        customNumberCurrentlyCalling = null;
                        isCallingCustom = false;
                    }
                }
            }));
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
                        if (customNumber.itemsToOrder != null && customNumber.itemsToOrder.Count > 0)
                        {
                            foreach (GameObject item in customNumber.itemsToOrder)
                            {
                                GameObject spawnedItem = GameObject.Instantiate(item);
                                spawnedItem.transform.position = new Vector3(-1711.802f, 3.518661f, 924.8834f);
                            }
                        }
                        customNumber.isOrdered = false;
                    }
                }
            }
        }
    }
}
