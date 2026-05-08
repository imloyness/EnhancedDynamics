using System.Collections;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal; 
using UnityEngine.SceneManagement;

namespace EnhancedDynamics.BBP
{
    public static class PostProcessing_BBP
    {
        private static Volume globalVolume;
        private static bool initialized = false;

        public static bool IsEnabled => BasePlugin.PostProcessingToggle.Value;
        public static float BloomVal => Mathf.Clamp01(BasePlugin.BloomIntensity.Value / 5f);
        public static float VignetteVal => Mathf.Clamp01(BasePlugin.VignetteIntensity.Value / 5f);
        public static float CAVal => Mathf.Clamp01(BasePlugin.ChromaticAberrationIntensity.Value / 5f);
        public static float GrainVal => Mathf.Clamp01(BasePlugin.GrainIntensity.Value / 5f);
        public static float MotionVal => Mathf.Clamp01(BasePlugin.MotionBlurIntensity.Value / 5f);
        public static void Initialize()
        {
            if (initialized) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            initialized = true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Game")
                return;

            if (IsEnabled)
                BasePlugin.Instance.StartCoroutine(ApplyPostProcessing());
        }

        private static IEnumerator ApplyPostProcessing()
        {
            yield return new WaitForSeconds(0.1f); 

            if (globalVolume == null)
            {
                var volumeObj = new GameObject("ED_GlobalPostVolume");
                globalVolume = volumeObj.AddComponent<Volume>();
                globalVolume.isGlobal = true;
                globalVolume.priority = 10;
                globalVolume.profile = ScriptableObject.CreateInstance<VolumeProfile>();
            }

            Camera[] allCameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);

            foreach (Camera cam in allCameras)
            {
                if (cam.name != "MainCamera" && cam.name != "BillboardCamera") 
                {
                    continue;
                }

                var cameraData = cam.GetUniversalAdditionalCameraData();

                if (cameraData != null)
                {
                    cameraData.renderPostProcessing = true;
                    BasePlugin.logsblablabla.LogInfo($"Enhanced Dynamics | Enabled Post-Processing on: {cam.name}");
                }
            }

            var profile = globalVolume.profile;

            if (!profile.Has<MotionBlur>()) profile.Add<MotionBlur>(true);
            if (!profile.Has<Bloom>()) profile.Add<Bloom>(true);
            if (!profile.Has<Vignette>()) profile.Add<Vignette>(true);
            if (!profile.Has<ChromaticAberration>()) profile.Add<ChromaticAberration>(true);
            if (!profile.Has<FilmGrain>()) profile.Add<FilmGrain>(true);

            RefreshEffects();

            BasePlugin.logsblablabla.LogInfo("Enhanced Dynamics | URP Post-Processing applied successfully!");
        }

        public static void RefreshEffects()
        {
            if (globalVolume == null)
            {
                return;
            }

            globalVolume.enabled = IsEnabled;

            if (!IsEnabled) return;

            var profile = globalVolume.profile;

            if (profile.TryGet(out MotionBlur mb))
            {
                mb.active = true;
                mb.intensity.Override(1f);
            }

            if (profile.TryGet(out Bloom bloom))
            {
                bloom.active = true;
                bloom.intensity.Override(BloomVal);
                bloom.threshold.Override(5.1f);
                bloom.scatter.Override(0.5f);
            }

            if (profile.TryGet(out Vignette vignette))
            {
                vignette.active = true;
                vignette.intensity.Override(VignetteVal);
                vignette.smoothness.Override(0.8f);
            }

            if (profile.TryGet(out ChromaticAberration ca))
            {
                ca.active = true;
                ca.intensity.Override(CAVal);
            }

            if (profile.TryGet(out FilmGrain grain))
            {
                grain.active = true;
                grain.intensity.Override(GrainVal);
                grain.type.Override(FilmGrainLookup.Medium1);
            }
        }
    }
}