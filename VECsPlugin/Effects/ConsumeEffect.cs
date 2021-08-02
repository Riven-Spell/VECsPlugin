using UnityEngine;

namespace VECsPlugin.Effects
{
    public class ConsumeEffect : MonoBehaviour, GameTemporaryEffect
    {
        public const float ConsumeIncreaseDegree = 0.5f; // % of a bullet's damage

        private float ConsumeDegree = 0f;
        private CharacterData data;

        public void PrepareOnce(CharacterData data)
        {
            this.data = data;
            data.block.BlockProjectileAction += OnBlockProjectileAction;
        }
        
        private void OnBlockProjectileAction(GameObject o, Vector3 fwd, Vector3 pos)
        {
            UnityEngine.Debug.Log("Blocked projectile");
            var phit = o.GetComponent<ProjectileHit>();
            data.healthHandler.Heal(phit.damage * ConsumeDegree);

            UnityEngine.Object.Destroy(o);
        } 
        
        public void IncreaseConsumeDegree(float by)
        {
            ConsumeDegree += by;
        }

        public void EndGame()
        {
            data.block.BlockProjectileAction -= OnBlockProjectileAction;
        }
    }
}