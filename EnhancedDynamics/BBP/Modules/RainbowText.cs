//funsies
using TMPro;
using UnityEngine;

namespace EnhancedDynamics
{
    public class RainbowText : MonoBehaviour
    {
        private TextMeshProUGUI tmpText;
        private float hue = 0f;
        public float speed = 0.3f;

        void Start()
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            hue = (Time.unscaledTime * speed) % 1f;

            tmpText.ForceMeshUpdate();
            var textInfo = tmpText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                Color32[] newColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                float characterOffset = i * 0.1f;
                float finalHue = Mathf.Repeat(hue + characterOffset, 1f);

                Color rainbowColor = Color.HSVToRGB(finalHue, 1f, 1f);

                newColors[vertexIndex + 0] = rainbowColor;
                newColors[vertexIndex + 1] = rainbowColor;
                newColors[vertexIndex + 2] = rainbowColor;
                newColors[vertexIndex + 3] = rainbowColor;
            }

            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }
}