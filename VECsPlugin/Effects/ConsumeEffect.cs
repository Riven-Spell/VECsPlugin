using UnityEngine;

namespace VECsPlugin.Effects
{
    public class ConsumeEffect : MonoBehaviour, GameTemporaryEffect
    {
        public const float ConsumeIncreaseDegree = 0.5f; // % of a bullet's damage

        private float ConsumeDegree = 0f;

        public void PrepareOnce(CharacterData data)
        {
            data.block.BlockProjectileAction += (o, vector3, arg3) =>
            {
                UnityEngine.Debug.Log("Blocked projectile");
                var phit = o.GetComponent<ProjectileHit>();
                data.healthHandler.Heal(phit.damage * ConsumeDegree);

                UnityEngine.Object.Destroy(o);
            };
        }
        
        public void IncreaseConsumeDegree(float by)
        {
            ConsumeDegree += by;
        }
        
        public void EndGame() { }
    }
}