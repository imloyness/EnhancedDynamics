using UnityEngine;
using MTM101BaldAPI.UI;
using TMPro;

namespace EnhancedDynamics.BBP
{
    internal class DebugUI
    {
        public static void ApplyDebugUI()
        {
            Canvas debugCanvas = UIHelpers.CreateBlankUIScreen("ED_DebugUI", true, true);
            
            TextMeshProUGUI textMeshProUGUI = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.BoldComicSans12, "\n<color=#FFFFFF80>" + " v." + PluginInfo.PLUGIN_VERSION + "</color>", debugCanvas.transform, Vector3.zero);
            textMeshProUGUI.alignment = TextAlignmentOptions.BottomLeft;
        }
    }
}
