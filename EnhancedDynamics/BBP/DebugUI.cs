using MTM101BaldAPI.UI;
using TMPro;
using UnityEngine;

namespace EnhancedDynamics.BBP
{
    internal static class DebugUI
    {
        private static TextMeshProUGUI debugText;

        public static void ApplyDebugUI(){
            Canvas debugCanvas = UIHelpers.CreateBlankUIScreen("ED_DebugUI", true, true);

            debugText = UIHelpers.CreateText<TextMeshProUGUI>(
                    BaldiFonts.BoldComicSans12, 
                    "Initializing...", 
                    debugCanvas.transform, 
                    Vector3.zero
                );
                debugText.alignment = TextAlignmentOptions.BottomLeft;

            debugCanvas.gameObject.AddComponent<DebugUpdater>();
        }

        internal class DebugUpdater : MonoBehaviour
        {
            void Update()
            {
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
                debugText.text = $"<color=#FFFFFF80>ED v.{PluginInfo.PLUGIN_VERSION}</color>\n" +
                                 $"Scene: <color=#FFFF00>{sceneName}</color>\n";
            }
        }
    }
}
