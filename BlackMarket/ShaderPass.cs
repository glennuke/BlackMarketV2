using MSCLoader;
using UnityEngine;

namespace BlackMarketV2
{
    public class ShaderPass : MonoBehaviour
    {
        public Material effectMaterial;

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (effectMaterial != null)
            {
                Graphics.Blit(src, dest, effectMaterial);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }
    }
}