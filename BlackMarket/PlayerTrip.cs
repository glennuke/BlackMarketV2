using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace BlackMarketV2
{
    public class PlayerTrip : MonoBehaviour
    {
        internal bool isPlayerHigh;
        internal DrugType highOnType;
        internal float wearOffTime;
        internal float originalFOV;

        internal static PlayerTrip Instance;

        private Tonemapping tonemapping;

        private Camera cam;

        void Start()
        {
            Instance = this;
            cam = Camera.main;
            tonemapping = cam.GetComponent<Tonemapping>();
        }

        internal void GetHigh()
        {
            if (highOnType == DrugType.Cocaine)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerStress").Value *= 0.25f;
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerDrunkAdjusted").Value += 0.5f;
                wearOffTime = 300;
            }

            if (highOnType == DrugType.Amphetamine)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerHunger").Value *= 0.25f;
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerStress").Value *= 0.5f;
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerFatigue").Value *= 0.25f;
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerDrunkAdjusted").Value += 0.3f;
                wearOffTime = 600;
            }

            if (highOnType == DrugType.Methamphetamine)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerStress").Value *= 0.1f;
                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerDrunkAdjusted").Value += 0.3f;
                wearOffTime = 900;
            }

            if (highOnType == DrugType.Shrooms)
            {
                wearOffTime = 200;
                originalFOV = cam.fieldOfView;
                var comp = cam.GetComponent("ContrastEnhance");
                if (comp == null) return;

                Type contrastEnhanceType = comp.GetType();
                FieldInfo intensityField = contrastEnhanceType.GetField("intensity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (intensityField != null)
                {
                    intensityField.SetValue(comp, 10);
                }
                cam.GetComponent<ShaderPass>().enabled = true;
                StartCoroutine(ShroomsTripFOVCoroutine());
                StartCoroutine(ShroomsTripTonemappingCoroutine());
            }
        }

        void Update()
        {
            if (isPlayerHigh)
            {
                if (highOnType == DrugType.Shrooms)
                {
                    UpdateShroomsTrip();
                }

                if (wearOffTime > 0)
                {
                    wearOffTime -= Time.deltaTime;
                }
                else
                {
                    isPlayerHigh = false;
                    StopCoroutine(ShroomsTripFOVCoroutine());
                    StopCoroutine(ShroomsTripTonemappingCoroutine());
                    if (highOnType == DrugType.Shrooms)
                    {
                        cam.fieldOfView = originalFOV;
                        tonemapping.exposureAdjustment = 1;
                        var comp = cam.GetComponent("ContrastEnhance");
                        if (comp == null) return;

                        Type contrastEnhanceType = comp.GetType();
                        FieldInfo intensityField = contrastEnhanceType.GetField("intensity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (intensityField != null)
                        {
                            intensityField.SetValue(comp, 1);
                        }
                        cam.GetComponent<ShaderPass>().enabled = false;
                    }
                }
            }
        }

        IEnumerator ShroomsTripFOVCoroutine()
        {
            while (isPlayerHigh)
            {
                float targetFOV = UnityEngine.Random.Range(25f, 40f);
                float lerpSpeed = UnityEngine.Random.Range(0.2f, 0.5f);

                while (Mathf.Abs(cam.fieldOfView - targetFOV) > 0.1f)
                {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * lerpSpeed);
                    yield return null;
                }

                PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerMovementSpeed").Value = UnityEngine.Random.Range(0.2f, 5f);

                cam.fieldOfView = 120f;

                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
            }
        }

        IEnumerator ShroomsTripTonemappingCoroutine()
        {
            while (isPlayerHigh)
            {
                yield return new WaitForSeconds(0.1f);
                tonemapping.exposureAdjustment = 10;
                yield return new WaitForSeconds(0.1f);
                tonemapping.exposureAdjustment = 5;
                yield return new WaitForSeconds(0.1f);
                tonemapping.exposureAdjustment = 1;
            }
        }

        void UpdateShroomsTrip()
        {
            
        }
    }
}