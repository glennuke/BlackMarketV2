using MSCLoader;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackMarketV2
{
    public class BlackMarket : Mod
    {
        public override string ID
        {
            get
            {
                return "BlackMarketV2";
            }
        }

        public override string Name
        {
            get
            {
                return "Black Market V2";
            }
        }

        public override string Author
        {
            get
            {
                return "0387";
            }
        }

        public override string Version
        {
            get
            {
                return "1.0";
            }
        }

        public override string Description
        {
            get
            {
                return "Buy/Sell drugs";
            }
        }

        public override Game SupportedGames
        {
            get
            {
                return Game.MyWinterCar;
            }
        }

        public GameObject CashPrefab;
        public GameObject DrugBoxPrefab;

        public GameObject CocaineBagPrefab;
        public GameObject AmphetamineBagPrefab;
        public GameObject MethamphetamineBagPrefab;
        public GameObject ShroomsPrefabBagPrefab;

        private Camera cam;

        internal int reputation;

        internal Text topLabelUI;

        public static BlackMarket Instance;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.OnSave, Mod_OnSave);
        }

        private void Mod_OnLoad()
        {
            Instance = this;
            if (SaveLoad.ValueExists(this, "reputation"))
            {
                reputation = SaveLoad.ReadValue<int>(this, "reputation");
            }
            cam = Camera.main;

            AssetBundle ab = LoadAssets.LoadBundle("BlackMarketV2.Assets.blackmarket.unity3d");
            GameObject canvas = GameObject.Instantiate<GameObject>(ab.LoadAsset<GameObject>("BlackMarketCanvas"));
            topLabelUI = canvas.transform.Find("LabelTop").GetComponent<Text>();

            CashPrefab = ab.LoadAsset<GameObject>("Cash");

            DrugBoxPrefab = ab.LoadAsset<GameObject>("DrugBox");

            CocaineBagPrefab = ab.LoadAsset<GameObject>("Cocaine");
            //AmphetamineBagPrefab = ab.LoadAsset<GameObject>("Amphetamine");
            //MethamphetamineBagPrefab = ab.LoadAsset<GameObject>("Methamphetamine");
            //ShroomsPrefabBagPrefab = ab.LoadAsset<GameObject>("Shrooms");

            cam.gameObject.AddComponent<PlayerTrip>();
            ShaderPass shaderPass = cam.gameObject.AddComponent<ShaderPass>();
            shaderPass.effectMaterial = ab.LoadAsset<Material>("shroomtripMat");
            shaderPass.enabled = false;

            // example code for making openable drug bos (placeholder coke model, theres only coke for now)
            GameObject testCokeBag = GameObject.Instantiate(CocaineBagPrefab);
            testCokeBag.GetComponent<DrugBag>().weight = 1f;
            testCokeBag.SetActive(false);

            GameObject testDrugBox = GameObject.Instantiate(DrugBoxPrefab);
            testDrugBox.transform.position = new Vector3(-1711.802f, 3.518661f, 924.8834f);
            testDrugBox.GetComponent<DrugBox>().bagsInside.Add(testCokeBag);

            // example code for making an order phone number
            List<GameObject> testOrder = new List<GameObject>()
            {
                testDrugBox
            };
            PhoneHandler.AddOrder(Random.Range(455555, 599999).ToString(), testOrder, 500f);
        }

        private void Mod_Update()
        {
            
        }

        private void Mod_OnSave()
        {
            SaveLoad.WriteValue<int>(this, "reputation", reputation);
        }
    }
}
