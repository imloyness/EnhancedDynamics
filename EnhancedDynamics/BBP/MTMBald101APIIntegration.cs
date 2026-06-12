using MTM101BaldAPI.OptionsAPI;
using UnityEngine;

namespace EnhancedDynamics.BBP
{
    internal static class MTMBald101APIIntegration
    {
        public static void TryIntegrate()
        {
            var singletonThing = new GameObject("randomSingleton");
            singletonThing.AddComponent<DefaultGeneralSettings>();
            
            CustomOptionsCore.OnMenuInitialize += AddCategory;
        }

        static void AddCategory(OptionsMenu __instance, CustomOptionsHandler handler)
        {
            handler.AddCategory<GeneralSettings>("Enhanced\nDynamics");
            handler.AddCategory<PostProcessingSettings>("Post\nProcessing");
        }
    }
}