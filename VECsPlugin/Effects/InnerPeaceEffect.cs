using UnboundLib;
using UnityEngine;
using VECsPlugin.Cards;

namespace VECsPlugin.Effects
{
    public class InnerPeaceEffect : MonoBehaviour, RoundTemporaryEffect
    {
        private Player p;
        private PlayerActions pa;
        private Gun g;
        private ParticleSystem ps;
        private ParticleSystemRenderer psr;

        private Vector3 lastPosition;

        private float MaxMultiplier;

        private bool HasFired;
        private float TimeWaited;

        private bool prepared;

        public void PrepareOnce(Player player, PlayerActions actions, Gun gun)
        {
            if (prepared)
                return;

            g = gun;
            p = player;
            pa = actions;
            var pso = Instantiate(new GameObject(), gun.transform);
            pso.transform.position += new Vector3(0, 0, 5);
            ps = pso.AddComponent<ParticleSystem>();
            psr = ps.GetComponent<ParticleSystemRenderer>();
            
            PrepareParticleSystem();
            
            g.ShootPojectileAction += OnShootProjectileAction;
            
            prepared = true;
        }

        private void PrepareParticleSystem()
        {
            var main = ps.main;
            main.duration = 5;
            main.startSpeed = -4f;
            main.startLifetime = .25f;
            main.startSize = 0.1f;
            var emission = ps.emission;
            emission.enabled = true;
            emission.rateOverTime = 100;
            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 1;
            shape.radiusThickness = 0;
            
            // this has purple debug particles... but I kinda like how it looks?
        }

        private void OnShootProjectileAction(GameObject o)
        {
            HasFired = true;
            
            var phit = o.GetComponentInChildren<ProjectileHit>();
            var mult = Mathf.Lerp(1, MaxMultiplier, TimeWaited / InnerPeace.MaxDuration);
            phit.damage *= mult;
            
            UnityEngine.Debug.Log($"Shot gun with damage multiplier {mult}");
        }

        public void AddMultiplier(float f)
        {
            MaxMultiplier += f;
        }

        private void Update()
        {
            var isMoving = pa.Left.IsPressed || pa.Right.IsPressed || pa.Up.IsPressed || pa.Down.IsPressed || pa.Jump.IsPressed;
            var emission = ps.emission;
            
            switch (g.isReloading)
            {
                case false when !isMoving && !HasFired && TimeWaited < InnerPeace.MaxDuration:
                    emission.enabled = true;
                    TimeWaited = Mathf.Clamp(TimeWaited + Time.deltaTime, 0f, InnerPeace.MaxDuration);
                    UnityEngine.Debug.Log($"Waited {TimeWaited}");
                    break;
                case true:
                    emission.enabled = false;
                    HasFired = false;
                    TimeWaited = 0;
                    break;
                default:
                    emission.enabled = false;
                    break;
            }

            emission.rateOverTime = Mathf.Lerp(0, 100, TimeWaited / InnerPeace.MaxDuration);
        }

        public void EndGame()
        {
            MaxMultiplier = 0;
            TimeWaited = 0f;
            
            DestroyImmediate(ps.gameObject);

            g.ShootPojectileAction -= OnShootProjectileAction;
        }

        public void SetupRound()
        {
            TimeWaited = 0f;
        }

        public void CleanupRound()
        {
            TimeWaited = 0f;
        }
    }
}