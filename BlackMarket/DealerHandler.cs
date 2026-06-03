using MSCLoader;
using UnityEngine;

namespace BlackMarketV2
{
    public class DealerHandler : MonoBehaviour
    {
        public float timerToDeactivate = 180f;
        private float originalTimerToDeactivate;
        bool playerArrived;
        GameObject Player;

	    void Start()
        {
            Player = GameObject.Find("PLAYER");
            if (timerToDeactivate == 0) timerToDeactivate = 180f;
            originalTimerToDeactivate = timerToDeactivate;
            DeactivateDealer();
        }

        void OnEnable()
        {
            playerArrived = false;
            timerToDeactivate = originalTimerToDeactivate;
            Restock();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > 20)
            {
                if (playerArrived)
                    timerToDeactivate -= Time.deltaTime;
            }
            else
            {
                timerToDeactivate = originalTimerToDeactivate;
                playerArrived = true;
            }

            if (timerToDeactivate <= 0)
            {
                DeactivateDealer();
            }
        }

        public void Restock()
        {
            for (int boxes = 0; boxes < Random.Range(1, 5); boxes++)
            {
                GameObject drugBox = GameObject.Instantiate(BlackMarket.Instance.DrugBoxPrefab);
                drugBox.transform.parent = transform;
                drugBox.transform.localPosition = new Vector3(Random.Range(0, -1f), 1f, Random.Range(0, -1f));
                for (int drugs = 0; drugs < Random.Range(2, 4); drugs++)
                {
                    int typeOfDrug = Random.Range(0, 3);

                    if (typeOfDrug == 0)
                    {
                        GameObject drug = GameObject.Instantiate(BlackMarket.Instance.CocaineBagPrefab);
                        drug.GetComponent<DrugBag>().weight = Random.Range(0.1f, 1f); // the weight in grams
                        drug.GetComponent<DrugBag>().Init();
                        drugBox.GetComponent<DrugBox>().AddContent(drug);
                    }
                    else if (typeOfDrug == 1)
                    {
                        GameObject drug = GameObject.Instantiate(BlackMarket.Instance.AmphetamineBagPrefab);
                        drug.GetComponent<DrugBag>().weight = Random.Range(0.1f, 4f); // the weight in grams
                        drug.GetComponent<DrugBag>().Init();
                        drugBox.GetComponent<DrugBox>().AddContent(drug);
                    }
                    else if (typeOfDrug == 2)
                    {
                        GameObject drug = GameObject.Instantiate(BlackMarket.Instance.MethamphetamineBagPrefab);
                        drug.GetComponent<DrugBag>().weight = Random.Range(0.1f, 2f); // the weight in grams
                        drug.GetComponent<DrugBag>().Init();
                        drugBox.GetComponent<DrugBox>().AddContent(drug);
                    }
                    else if (typeOfDrug == 3)
                    {
                        
                    }
                }
            }
        }

        public void DeactivateDealer()
        {
            gameObject.SetActive(false);
        }
    }
}