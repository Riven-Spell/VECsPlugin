using UnityEngine;
using VECsPlugin.Effects.Bullet;

namespace VECsPlugin.Effects
{
    public class TeleporterEffect : MonoBehaviour, GameTemporaryEffect
    {
        private Gun gun;
        private Player player;
        private bool didBlock = false;
        private Color oldColor;

        public void PrepareOnce(Player p, Gun g)
        {
            player = p;
            gun = g;

            player.data.block.BlockAction += OnBlock;
            gun.ShootPojectileAction += OnShootProjectileAction;
        }

        private void OnShootProjectileAction(GameObject o)
        {
            if (!didBlock) return;
                
            var effect = o.AddComponent<TeleportEffect>();
            effect.playerTransform = player.transform;

            gun.projectileColor = oldColor;
            didBlock = false;
        }
        
        private void OnBlock(BlockTrigger.BlockTriggerType btt)
        {
            didBlock = true;
            oldColor = gun.projectileColor;
            gun.projectileColor = Color.cyan;            
        }
        
        public void EndGame()
        {
            player.data.block.BlockAction -= OnBlock;
            gun.ShootPojectileAction -= OnShootProjectileAction;
        }
    }
}