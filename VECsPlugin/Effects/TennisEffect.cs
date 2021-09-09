using UnboundLib;
using UnityEngine;
using VECsPlugin.Patches;

namespace VECsPlugin.Effects
{
    public class TennisEffect : MonoBehaviour, RoundTemporaryEffect
    {
        private float _additionalDamage = 0f;

        private Player _p;
        private Gun _g;
        
        public void PrepareOnce(Player player, Gun gun)
        {
            _g = gun;
            _p = player;

            _g.ShootPojectileAction += OnShootProjectileAction;
            _p.data.block.BlockProjectileAction += OnBlockAction;
            _p.gameObject.GetOrAddComponent<BlockOwnProjectileHookEffect>().OnBlockOwnProjectile += OnBlockAction;
        }

        private void OnShootProjectileAction(GameObject o)
        {
            var phit = o.GetComponentInChildren<ProjectileHit>();
            phit.damage += _additionalDamage;
            
            UnityEngine.Debug.Log($"Shot a projectile with {phit.damage} damage ({_additionalDamage} extra)");
            
            _additionalDamage = 0;
        }

        private void OnBlockAction(GameObject o, Vector3 vector3, Vector3 arg3)
        {
            var phit = o.GetComponent<ProjectileHit>();
            _additionalDamage += phit.damage;
            
            UnityEngine.Object.Destroy(o);
        }
        
        public void EndGame()
        {
            _additionalDamage = 0;

            _g.ShootPojectileAction -= OnShootProjectileAction;
            _p.data.block.BlockProjectileAction -= OnBlockAction;
            // no need to worry about the extra hook, that gets cleaned up because it's a GameTemporaryEffect
        }

        public void SetupRound()
        {
            _additionalDamage = 0;
        }

        public void CleanupRound()
        {
            _additionalDamage = 0;
        }
    }
}