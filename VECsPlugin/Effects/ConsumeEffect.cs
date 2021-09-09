using UnityEngine;

namespace VECsPlugin.Effects
{
    public class ConsumeEffect : MonoBehaviour, GameTemporaryEffect
    {
        public const float ConsumeIncreaseDegree = 0.5f; // % of a bullet's damage

        private float ConsumeDegree = 0f;
        private CharacterData data;

        private bool prepared = false;

        public void PrepareOnce(CharacterData data)
        {
            if (prepared)
                return;
            
            this.data = data;
            data.block.BlockProjectileAction += OnBlockProjectileAction;

            prepared = true;
        }
        
        private void OnBlockProjectileAction(GameObject o, Vector3 fwd, Vector3 pos)
        {
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