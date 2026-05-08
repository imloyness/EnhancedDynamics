using HarmonyLib;

namespace EnhancedDynamics.BBP.Patches
{
    [HarmonyPatch]
    public static class PlayerMovementPatches_BBP
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        [HarmonyPostfix]
        public static void PostfixPlayerMovementUpdate(PlayerMovement __instance)
        {
            BasePlugin.Stamina_ED = __instance.stamina;
        }

    }
}