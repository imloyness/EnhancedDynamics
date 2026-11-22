using System.Collections;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnhancedDynamics
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; private set; }

        public static BepInEx.Logging.ManualLogSource logsblablabla = BepInEx.Logging.Logger.CreateLogSource("EnhancedDynamics");

        //configs
        public static ConfigEntry<int> FOVValue;
        public static ConfigEntry<int> CameraBobbingIntensity;

        public static ConfigEntry<bool> FOVToggle;
        public static ConfigEntry<bool> CameraBobbingToggle;
        public static ConfigEntry<bool> idleinhaleToggle;
        public static ConfigEntry<bool> VerticalCameraToggle;

        //public static ConfigEntry<bool> FootstepSoundsToggle;

        public static ConfigEntry<bool> DebugModeToggle;

        //shared variables
        public static bool FrozenState_ED;
        public static bool SlippingState_ED;
        public static float Velocity_ED;
        public static float Stamina_ED;

        private void Awake()
        {
            Instance = this;

            // config setup
            FOVValue = Config.Bind("Camera", "FOV Value", 1, "Set the FOV multiplier (1 = 30 FOV, 2 = 60 FOV, 3 = 90 FOV, 4 = 120 FOV)");
            CameraBobbingIntensity = Config.Bind("Camera", "Camera Bobbing Intensity", 2, "Set the Camera Bobbing Intensity");

            FOVToggle = Config.Bind("Camera", "FOV Toggle", true, "Enable/Disable FOV changes");
            CameraBobbingToggle = Config.Bind("Camera", "Camera Bobbing Toggle", true, "Enable/Disable Camera Bobbing");
            idleinhaleToggle = Config.Bind("Camera", "Idle Inhale Toggle", true, "Enable/Disable Idle Inhale Animation (Requires Camera Bobbing to be enabled.)");
            VerticalCameraToggle = Config.Bind("Camera", "Vertical Camera Toggle", true, "Enable/Disable Vertical Camera");

            //FootstepSoundsToggle = Config.Bind("Audio", "Footstep Sounds Toggle", true, "Enable/Disable Footstep Sounds");

            DebugModeToggle = Config.Bind("Debug", "Debug Mode Toggle", PluginInfo.isTest, "funsies");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //elephant hit
        }

        private IEnumerator Start()
        {
            yield return null;

            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            yield return new WaitForSeconds(1f);
            harmony.PatchAll(typeof(BBP.Patches.CameraPatches_BBP));
            harmony.PatchAll(typeof(BBP.Patches.NanaPeelPatches));
            harmony.PatchAll(typeof(BBP.Patches.PlayerMovementPatches_BBP));
            harmony.PatchAll(typeof(BBP.Patches.VerticalCameraFeature_BBP));
            if (Chainloader.PluginInfos.ContainsKey("mtm101.rulerp.bbplus.baldidevapi"))
            {
                logsblablabla.LogInfo("Enhanced Dynamics | MTM101BaldAPI detected, adding OptionsAPI integration.");
                BBP.MTMBald101APIIntegration.TryIntegrate();
            }

            logsblablabla.LogInfo("Enhanced Dynamics | " + PluginInfo.PLUGIN_NAME + " v." + PluginInfo.PLUGIN_VERSION + " | hi");
        }
    }
}
