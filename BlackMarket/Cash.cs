using UnityEngine;

namespace BlackMarketV2
{
    public class Cash : MonoBehaviour
    {
        public float moneyToGive;

        void OnMouseOver()
        {
            PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = true;
            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = string.Format("PAYMENT {0} MK", moneyToGive);
        }

        void OnMouseExit()
        {
            PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = false;
            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "";
        }

        void OnMouseDown()
        {
            PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerMoney").Value += moneyToGive;
            PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = false;
            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "";
            GameObject.Destroy(this);
        }
    }
}
