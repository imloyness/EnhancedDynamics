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
        public static float BloomVal => BasePlugin.BloomIntensity.Value;
        public static float VignetteVal => BasePlugin.VignetteIntensity.Value;
        public static float CAVal => BasePlugin.ChromaticAberrationIntensity.Value;
        public static float GrainVal => BasePlugin.GrainIntensity.Value;

        public static void Initialize()
        {
            if (initialized) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            initialized = true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (GameDetector.whatbbgame_ED != "bbp" || scene.name != "Game")
                return;

            if (scene.name == "MainMenu") return;

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

            var cameraData = Camera.main?.GetComponent<UniversalAdditionalCameraData>();
            if (cameraData != null)
            {
                cameraData.renderPostProcessing = true;
            }

            var profile = globalVolume.profile;

            if (!profile.Has<Bloom>()) profile.Add<Bloom>(true);
            if (!profile.Has<Vignette>()) profile.Add<Vignette>(true);
            if (!profile.Has<ChromaticAberration>()) profile.Add<ChromaticAberration>(true);
            if (!profile.Has<FilmGrain>()) profile.Add<FilmGrain>(true);

            RefreshEffects();

            BasePlugin.logsblablabla.LogInfo("Enhanced Dynamics | URP Post-Processing applied successfully!");
        }

        public static void RefreshEffects()
        {
            
            globalVolume.enabled = IsEnabled;

            if (!IsEnabled) return;

            var profile = globalVolume.profile;

            if (profile.TryGet(out Bloom bloom))
            {
                bloom.active = true;
                bloom.intensity.Override(BloomVal);
                bloom.threshold.Override(1.1f);
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