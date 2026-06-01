using MSCLoader;
using UnityEngine;

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
            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = string.Format("{0} {1} Grams",drugType, weight);
            if (Input.GetKeyDown("f"))
            {
				PlayerTrip.Instance.isPlayerHigh = true;
                PlayerTrip.Instance.highOnType = drugType;
                PlayerTrip.Instance.GetHigh();
				Destroy(gameObject);
			}
		}

		internal void Start()
		{
			gameObject.MakePickable();
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
		}

		private void Update()
		{
			position = transform.localPosition;
			rotation = transform.localEulerAngles;
		}
	}
}
