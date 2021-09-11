
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
        private Dictionary<GameObject, MonoBehaviour> MoveCache = new Dictionary<GameObject, MonoBehaviour>();

        public float GravWellForce = GravityWells.MaximumGravityWellForce;
        public float GravWellRadius = GravityWells.GravityWellRadius;

        public void FixedUpdate()
        {
            var colls = Physics2D.OverlapCircleAll(this.transform.position, GravWellRadius);
            foreach (var c2d in colls)
            {
                GameObject go;
                
                if (IgnoreCache.Contains(go = c2d.gameObject))
                    continue;

                {
                    MonoBehaviour ovel;
                    if (!MoveCache.TryGetValue(go, out ovel))
                    {
                        // Maybe it's a player?
                        ovel = go.GetComponentInChildren<PlayerVelocity>();
                        if (ovel != null)
                        {
                            MoveCache.Add(go, ovel);
                        }
                        else
                        {
                            // Maybe it's an object?
                            ovel = go.GetComponentInChildren<NetworkPhysicsObject>();
                            if (ovel == null)
                            {
                                // It's neither of our movable types. Cache it.
                                IgnoreCache.Add(c2d.gameObject);
                                continue;
                            }
                        }
                    }

                    if (ovel is PlayerVelocity)
                    {
                        pvelInvoker.Invoke((PlayerVelocity) ovel, (Vector2) (-((c2d.transform.position - transform.position).normalized * GravWellForce)), ForceMode2D.Impulse);
                    }
                    else if (ovel is NetworkPhysicsObject)
                    {
                        ((NetworkPhysicsObject) ovel).BulletPush(-((c2d.transform.position - transform.position).normalized * GravWellForce) * 5, Vector2.zero, null);
                    }
                }
            }
        }
    }
}