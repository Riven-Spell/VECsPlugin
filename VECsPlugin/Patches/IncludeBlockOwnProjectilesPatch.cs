using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Patches
{
    [HarmonyPatch(typeof(Block), "blocked")]
    public class IncludeBlockOwnProjectilesPatch
    {
        static void Postfix(GameObject projectile, Vector3 forward, Vector3 hitPos, Block __instance)
        {
            var attack = projectile.GetComponentInParent<SpawnedAttack>();
            if ((bool)(UnityEngine.Object)attack && attack.spawner.gameObject == __instance.transform.root.gameObject) 
            {
                __instance.transform.root.gameObject.GetComponentInChildren<BlockOwnProjectileHookEffect>()?.BlockOwnProjectile(projectile, forward,  hitPos);
            }
        }
    }
}