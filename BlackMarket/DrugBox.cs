using System.Collections.Generic;
using UnityEngine;
using MSCLoader;

namespace BlackMarketV2
{
    public class DrugBox : MonoBehaviour
    {
        public List<GameObject> bagsInside = new List<GameObject>();

        public bool bought;
        public float price;

    	void OnMouseOver()
        {
            if (!bought)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = $"BUY {price} MK";
            }

            if (Input.GetKeyDown("f"))
            {
                if (bought)
                {
                    OpenBox();
                }
                else
                {
                    Buy();
                }
            }
        }

        void Start()
        {
            gameObject.name = "Drug Box";
        }

        void Update()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public void AddContent(GameObject drugBag)
        {
            bagsInside.Add(drugBag);
            price += drugBag.GetComponent<DrugBag>().price;
            gameObject.name += " " + drugBag.name + " ";
        }

        public void Buy()
        {
            if (PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerMoney").Value >= price)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerMoney").Value -= price;
                gameObject.MakePickable();
                gameObject.name += " (Clone)";
                bought = true;
            }
        }

        public void OpenBox()
        {
            foreach (GameObject bag in bagsInside)
            {
                bag.SetActive(true);
                bag.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
    }
}