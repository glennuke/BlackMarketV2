using System.Collections.Generic;
using UnityEngine;
using MSCLoader;

namespace BlackMarketV2
{
    public class DrugBox : MonoBehaviour
    {
        public List<GameObject> bagsInside = new List<GameObject>();

    	void OnMouseOver()
        {
            if (Input.GetKeyDown("f"))
            {
                OpenBox();
            }
        }

        void Start()
        {
            gameObject.MakePickable();
        }

        void Update()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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