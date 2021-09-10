using UnityEngine;
using VECsPlugin.Effects.Bullet;

namespace VECsPlugin.Effects
{
    public class GravityWellEffect : MonoBehaviour, GameTemporaryEffect
    {
        private Gun _g;
        private bool _prepared;
        
        public void PrepareOnce(Gun g)
        {
            if (_prepared)
                return;
            
            _g = g;
            
            _g.ShootPojectileAction += ShootProjectileAction;

            _prepared = true;
        }

        private void ShootProjectileAction(GameObject obj)
        {
            // attach the trigger
            obj.AddComponent<GravityWellTrigger>();
        }

        public void EndGame()
        {
            _g.ShootPojectileAction -= ShootProjectileAction;
        }
    }
}