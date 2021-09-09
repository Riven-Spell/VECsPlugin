using System;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public class BlockOwnProjectileHookEffect : MonoBehaviour, GameTemporaryEffect
    {
        public event Action<GameObject, Vector3, Vector3> OnBlockOwnProjectile;

        public void BlockOwnProjectile(GameObject projectile, Vector3 forward, Vector3 hitpos)
        {
            OnBlockOwnProjectile?.Invoke(projectile, forward, hitpos);
        }

        public void EndGame() { }
    }
}