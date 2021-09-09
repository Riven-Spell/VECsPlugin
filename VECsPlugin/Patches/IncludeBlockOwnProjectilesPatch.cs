using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace VECsPlugin.Patches
{
    [HarmonyPatch(typeof(Block), "blocked")]
    public class IncludeBlockOwnProjectilesPatch
    {
        public static Dictionary<Player, Action<GameObject, Vector3, Vector3>> Subscriptions = new Dictionary<Player, Action<GameObject, Vector3, Vector3>>();

        static void Postfix(GameObject projectile, Vector3 forward, Vector3 hitPos, Block __instance)
        {
            var attack = projectile.GetComponentInParent<SpawnedAttack>();
            if ((bool)(UnityEngine.Object)attack && attack.spawner.gameObject == __instance.transform.root.gameObject &&
                Subscriptions.ContainsKey(attack.spawner))
                Subscriptions[attack.spawner](projectile, forward, hitPos);
        }
    }
}