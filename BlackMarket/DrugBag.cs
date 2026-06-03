using MSCLoader;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlackMarketV2
{
	public enum DrugType
	{
		Cocaine,
		Amphetamine,
		Methamphetamine,
		Shrooms
	}

	public class DrugBag : MonoBehaviour
	{
		public DrugType drugType;

		public float weight;

		public float price;

		public Vector3 position;

		public Vector3 rotation;
        void OnMouseOver()
        {
            if (Input.GetKeyDown("f"))
            {
				PlayerTrip.Instance.isPlayerHigh = true;
                PlayerTrip.Instance.highOnType = drugType;
                PlayerTrip.Instance.GetHigh();
				Destroy(gameObject);
			}
		}

		public void Init()
		{
			Math.Round(weight, 2);

            switch (drugType)
            {
                case DrugType.Cocaine:
                    price = Random.Range(70f, 250f) * weight;
                    break;
                case DrugType.Amphetamine:
                    price = Random.Range(50f, 100f) * weight;
                    break;
                case DrugType.Methamphetamine:
                    price = Random.Range(100f, 500f) * weight;
                    break;
                case DrugType.Shrooms:
                    price = Random.Range(30f, 80f) * weight;
                    break;
            }

			gameObject.name = $"{drugType} {weight}g (Clone)";
            gameObject.MakePickable();
            gameObject.SetActive(false);
        }

		void Update()
		{
			position = transform.localPosition;
			rotation = transform.localEulerAngles;
		}
	}
}
