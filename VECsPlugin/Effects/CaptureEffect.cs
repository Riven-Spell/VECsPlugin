using UnityEngine;
using VECsPlugin.Effects.Bullet;

namespace VECsPlugin.Effects
{
    public class CaptureEffect : MonoBehaviour, GameTemporaryEffect
    {
        private Player target;
        
        protected Gun gun;
        protected Player player;
        protected bool didBlock = false;
        protected Color oldColor;

        public void PrepareOnce(Player p, Gun g)
        {
            player = p;
            gun = g;

            player.data.block.BlockAction += OnBlock;
            gun.ShootPojectileAction += OnShootProjectileAction;
        }

        public void OnHitAction(HitInfo hitInfo)
        {
            if (didBlock && target != null)
                return;

            target = hitInfo.collider.GetComponentInChildren<Player>();
        }
        
        public void OnShootProjectileAction(GameObject o)
        {
            o.GetComponentInChildren<ProjectileHit>().AddHitActionWithData(OnHitAction);
            
            if (!didBlock || target == null) return;
                
            var effect = o.AddComponent<TeleportEffect>();
            effect.playerTransform = target.transform;

            gun.projectileColor = oldColor;
            didBlock = false;            
        }

        public void OnBlock(BlockTrigger.BlockTriggerType btt)
        {
            if (target == null)
                return; // can't set up capture without a "captured" opponent.
            
            didBlock = true;
            oldColor = gun.projectileColor;
            gun.projectileColor = Color.red;
        }
        
        public void EndGame()
        {
            player.data.block.BlockAction -= OnBlock;
            gun.ShootPojectileAction -= OnShootProjectileAction;
        }
    }
}