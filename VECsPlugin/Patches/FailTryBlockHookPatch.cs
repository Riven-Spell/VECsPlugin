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
                var effect = ___data.player.gameObject.GetComponentInChildren<FailTryBlockHookEffect>();

                if (effect != null)
                {
                    effect.FailedBlock(__instance);
                }
            }
        }
    }
}