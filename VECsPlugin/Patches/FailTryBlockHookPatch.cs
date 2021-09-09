using HarmonyLib;
using VECsPlugin.Effects;

namespace VECsPlugin.Patches
{
    [HarmonyPatch(typeof(Block), "TryBlock")]
    public class FailTryBlockHookPatch
    {
        static void Postfix(Block __instance, CharacterData ___data)
        {
            if (!__instance.IsBlocking() && __instance.counter < __instance.Cooldown())
            {
                ___data.player.gameObject.GetComponentInChildren<FailTryBlockHookEffect>()?.FailedBlock(__instance);
            }
        }
    }
}