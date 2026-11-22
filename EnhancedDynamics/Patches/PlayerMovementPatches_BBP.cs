using HarmonyLib;

namespace EnhancedDynamics.Patches
{
    [HarmonyPatch]
    public static class PlayerMovementPatches
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        [HarmonyPostfix]
        public static void PostfixPlayerMovementUpdate(PlayerMovement __instance)
        {
            BasePlugin.Stamina_ED = __instance.stamina;
        }

    }
}
