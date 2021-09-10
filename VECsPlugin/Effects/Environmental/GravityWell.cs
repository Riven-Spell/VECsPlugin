
using System;
using System.Collections.Generic;
using HarmonyLib;
using UnboundLib;
using UnityEngine;
using VECsPlugin.Cards;

namespace VECsPlugin.Effects.Environmental
{
    public class GravityWell : MonoBehaviour
    {
        private static FastInvokeHandler pvelInvoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(PlayerVelocity), "AddForce", new Type[] { typeof(Vector2), typeof(ForceMode2D) }));
        private HashSet<GameObject> IgnoreCache = new HashSet<GameObject>();
        private Dictionary<GameObject, PlayerVelocity> MoveCache = new Dictionary<GameObject, PlayerVelocity>();

        public void FixedUpdate()
        {
            var colls = Physics2D.OverlapCircleAll(this.transform.position, GravityWells.GravityWellRadius);
            foreach (var c2d in colls)
            {
                GameObject go;
                
                if (IgnoreCache.Contains(go = c2d.gameObject))
                    continue;

                {
                    // Maybe it's a player?
                    PlayerVelocity pvel;
                    if (!MoveCache.TryGetValue(go, out pvel))
                    {
                        pvel = go.GetComponentInChildren<PlayerVelocity>();
                        MoveCache.Add(go, pvel);
                    }
                    
                    if (pvel != null)
                    {
                        pvelInvoker.Invoke(pvel, (Vector2) (-((c2d.transform.position - transform.position).normalized * GravityWells.GravityWellForce)), ForceMode2D.Impulse);
                    }
                    else
                    {
                        IgnoreCache.Add(c2d.gameObject);
                    }
                }
            }
        }
    }
}