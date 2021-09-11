using UnityEngine;
using VECsPlugin.Cards;
using VECsPlugin.Effects.Bullet;
using VECsPlugin.Effects.Environmental;

namespace VECsPlugin.Effects
{
    public class GravityWellEffect : MonoBehaviour, GameTemporaryEffect
    {
        private Gun _g;
        private GunAmmo _ga;
        private bool _prepared;

        private bool reverse;
        private int stacks;
        
        public void PrepareOnce(Gun g, GunAmmo ga, bool isReverse = false)
        {
            if (_prepared)
                return;
            
            _g = g;
            _ga = ga;
            reverse = isReverse;
            
            _g.ShootPojectileAction += ShootProjectileAction;

            _prepared = true;
        }

        public void AddStack()
        {
            stacks++;
        }

        private void ShootProjectileAction(GameObject obj)
        {
            // attach the trigger
            var trigger = obj.AddComponent<GravityWellTrigger>();
            trigger.GravWellForce = Mathf.Clamp(GravityWells.GravityWellDistributedForce / _ga.maxAmmo, 0f, GravityWells.MaximumGravityWellForce);
            trigger.GravWellForce *= reverse ? -3 : 1;
            trigger.GravWellRadius = (1f + ((stacks - 1) * .5f)) * GravityWells.GravityWellRadius;
        }

        public void EndGame()
        {
            _g.ShootPojectileAction -= ShootProjectileAction;
        }
    }
}