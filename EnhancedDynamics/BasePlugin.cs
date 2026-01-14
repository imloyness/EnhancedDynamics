using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using BepInEx.Bootstrap;
using System.Collections;
using UnityEngine;

namespace EnhancedDynamics
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource logsblablabla = BepInEx.Logging.Logger.CreateLogSource("EnhancedDynamics");

        //configs
        public static ConfigEntry<int> FOVValue;
        public static ConfigEntry<int> CameraBobbingIntensity;

        public static ConfigEntry<bool> FOVToggle;
        public static ConfigEntry<bool> CameraBobbingToggle;
        public static ConfigEntry<bool> idleinhaleToggle;
        public static ConfigEntry<bool> VerticalCameraToggle;

        public static ConfigEntry<bool> FootstepSoundsToggle;

        public static ConfigEntry<bool> DebugModeToggle;


        //shared variables
        public static bool FrozenState_ED;
        public static bool SlippingState_ED;
        public static float Velocity_ED;
        public static float Stamina_ED;

        private void Awake()
        {
            // config setup
            FOVValue = Config.Bind("Camera", "FOV Value", 1, "Set the FOV multiplier (1 = 30 FOV, 2 = 60 FOV, 3 = 90 FOV, 4 = 120 FOV)");
            CameraBobbingIntensity = Config.Bind("Camera", "Camera Bobbing Intensity", 2, "Set the Camera Bobbing Intensity");

            FOVToggle = Config.Bind("Camera", "FOV Toggle", true, "Enable/Disable FOV changes");
            CameraBobbingToggle = Config.Bind("Camera", "Camera Bobbing Toggle", true, "Enable/Disable Camera Bobbing");
            idleinhaleToggle = Config.Bind("Camera", "Idle Inhale Toggle", true, "Enable/Disable Idle Inhale Animation (Requires Camera Bobbing to be enabled.)");
            VerticalCameraToggle = Config.Bind("Camera", "Vertical Camera Toggle", true, "Enable/Disable Vertical Camera");

            FootstepSoundsToggle = Config.Bind("Audio", "Footstep Sounds Toggle", true, "Enable/Disable Footstep Sounds");

            DebugModeToggle = Config.Bind("Debug", "Debug Mode Toggle", PluginInfo.isTest, "funsies");
        }

        private IEnumerator Start()
        {
            yield return null;

            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            GameDetector.DetectGame();
            if (GameDetector.whatbbgame_ED == "bbp")
            {
                yield return new WaitForSeconds(0.3f); //wait for mtmapi and other plugins to load
                harmony.PatchAll(typeof(CameraPatches_BBP));
                harmony.PatchAll(typeof(NanaPeelPatches));
                harmony.PatchAll(typeof(PlayerMovementPatches_BBP));
                harmony.PatchAll(typeof(VerticalCameraFeature_BBP));
                if (Chainloader.PluginInfos.ContainsKey("mtm101.rulerp.bbplus.baldidevapi"))
                {
                    logsblablabla.LogInfo("Enhanced Dynamics | MTM101BaldAPI detected, adding OptionsAPI integration.");
                    MTMBald101APIIntegration.TryIntegrate();
                }
            }
            if (GameDetector.whatbbgame_ED == "bbcr")
            {
                logsblablabla.LogWarning("Enhanced Dynamics | This plugin does not yet support BBCR, skip");
            }
            else if (GameDetector.whatbbgame_ED == "bbc")
            {
                logsblablabla.LogWarning("Enhanced Dynamics | This plugin does not yet support BBC, skip");
            }

            logsblablabla.LogInfo("Enhanced Dynamics | " + PluginInfo.PLUGIN_NAME + " v." + PluginInfo.PLUGIN_VERSION + " | hi");
        }
    }
}