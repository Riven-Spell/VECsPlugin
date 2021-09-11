using System.Linq;
using UnityEngine;
using VECsPlugin.Cards;
using VECsPlugin.Effects.Environmental;

namespace VECsPlugin.Effects.Bullet
{
    public class GravityWellTrigger : RayHitEffect
    {
        private static GameObject lineEffect = null;
        private static Color InColor = Color.magenta;
        private static Color OutColor = Color.red;

        public float GravWellForce = 100f;
        public float GravWellRadius = GravityWells.GravityWellRadius;
        
        public override HasToReturn DoHitEffect(HitInfo hit)
        {
            // build gravity well
            var gravWellGO = new GameObject();
            gravWellGO.transform.position = hit.point;
            // kill it after GravityWellTime
            var afterSeconds = gravWellGO.AddComponent<RemoveAfterSeconds>();
            afterSeconds.seconds = GravityWells.GravityWellTime;
            // attach spawned attack
            GetComponent<SpawnedAttack>().CopySpawnedAttackTo(gravWellGO);
            // set up GravityWell
            var gwell = gravWellGO.AddComponent<GravityWell>();
            gwell.GravWellForce = GravWellForce;
            gwell.GravWellRadius = GravWellRadius;
            // TODO: LineEffect
            if (lineEffect == null)
                FindLineEffect();
            
            // Steal the LineEffect from A_ChillingPresence
            var effectGO = Instantiate(lineEffect, gravWellGO.transform);
            var effect = effectGO.GetComponentInChildren<LineEffect>();
            effect.colorOverTime = new Gradient()
            {
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1, 0)
                },
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(GravWellForce > 0 ? InColor : OutColor, 0)
                },
                mode = GradientMode.Fixed
            };
            effect.widthMultiplier = 2f;
            effect.radius = GravWellRadius;
            effect.raycastCollision = false;
            effect.useColorOverTime = true;
            
            // set up particle system
            var _ps = effectGO.AddComponent<ParticleSystem>();
            var main = _ps.main;
            main.duration = 5;
            main.startSpeed = GravWellForce > 0 ? -4f : 4f;
            main.startLifetime = .5f;
            main.startSize = 0.1f;
            var emission = _ps.emission;
            emission.enabled = true;
            emission.rateOverTime = 150;
            var shape = _ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = GravWellForce > 0 ? GravWellRadius + 0.25f : GravWellRadius - 0.5f;
            shape.radiusThickness = 0;
            var _psr = effectGO.GetComponentInChildren<ParticleSystemRenderer>();
            if (GravWellForce < 0) // because the brighter missing texture actually looks really good
                _psr.material.color = OutColor;

            return HasToReturn.canContinue;
        }

        private void FindLineEffect()
        {
            UnityEngine.Debug.Log($"{CardChoice.instance.cards.Length}");

            var card = CardChoice.instance.cards.First(c => c.name.Equals("ChillingPresence"));
            var statMods = card.gameObject.GetComponentInChildren<CharacterStatModifiers>();
            lineEffect = statMods.AddObjectToPlayer.GetComponentInChildren<LineEffect>().gameObject;
        }
    }
}