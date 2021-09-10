using System.Linq;
using UnboundLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VECsPlugin.Cards;
using VECsPlugin.Effects.Environmental;

namespace VECsPlugin.Effects.Bullet
{
    public class GravityWellTrigger : RayHitEffect
    {
        private static GameObject lineEffect = null;   
        
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
            gravWellGO.AddComponent<GravityWell>();
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
                    new GradientColorKey(Color.magenta, 0)
                },
                mode = GradientMode.Fixed
            };
            effect.widthMultiplier = 2f;
            effect.radius = GravityWells.GravityWellRadius;
            effect.raycastCollision = false;
            effect.useColorOverTime = true;
            
            // set up particle system
            var _ps = effectGO.AddComponent<ParticleSystem>();
            var main = _ps.main;
            main.duration = 5;
            main.startSpeed = -4f;
            main.startLifetime = .5f;
            main.startSize = 0.1f;
            var emission = _ps.emission;
            emission.enabled = true;
            emission.rateOverTime = 150;
            var shape = _ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = GravityWells.GravityWellRadius + 0.25f;
            shape.radiusThickness = 0;

            return HasToReturn.canContinue;
        }

        private void FindLineEffect()
        {
            UnityEngine.Debug.Log($"{CardChoice.instance.cards.Length}");
            
            // foreach (var card in CardChoice.instance.cards)
            // {
            //     var mods = card.gameObject.GetComponentInChildren<CharacterStatModifiers>();
            //     var hasLineEffect = mods.AddObjectToPlayer != null &&
            //                         mods.AddObjectToPlayer.GetComponentInChildren<LineEffect>() != null;
            //     UnityEngine.Debug.Log($"{card.name} has mods? {mods != null} adds something to character? {mods.AddObjectToPlayer != null} has line effect? {hasLineEffect}");
            // }

            var card = CardChoice.instance.cards.First(c => c.name.Equals("ChillingPresence"));
            UnityEngine.Debug.Log($"Card not null? {card != null}");
            var statMods = card.gameObject.GetComponentInChildren<CharacterStatModifiers>();
            UnityEngine.Debug.Log($"StatMods not null? {statMods != null}");
            UnityEngine.Debug.Log($"LineEffect exists? {statMods.AddObjectToPlayer.GetComponentInChildren<LineEffect>() != null}");
            UnityEngine.Debug.Log($"LineEffect has game object? {statMods.AddObjectToPlayer.GetComponentInChildren<LineEffect>().gameObject != null}");
            lineEffect = statMods.AddObjectToPlayer.GetComponentInChildren<LineEffect>().gameObject;
        }
    }
}