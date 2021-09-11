using UnboundLib;
using UnityEngine;
using VECsPlugin.Cards;

namespace VECsPlugin.Effects
{
    public class InnerPeaceEffect : MonoBehaviour, RoundTemporaryEffect
    {
        private Player _p;
        private Gun _g;
        private ParticleSystem _ps;
        private ParticleSystemRenderer _psr;
        private PlayerVelocity _pv;

        private float _maxMultiplier;

        private bool _hasFired;
        private float _timeWaited;

        private bool _prepared;

        public void PrepareOnce(Player player, Gun gun)
        {
            if (_prepared)
                return;

            _g = gun;
            _p = player;
            _pv = player.GetComponentInChildren<PlayerVelocity>();
            var pso = Instantiate(new GameObject(), gun.transform);
            pso.transform.position += new Vector3(0, 0, 5);
            _ps = pso.AddComponent<ParticleSystem>();
            _psr = pso.GetComponentInChildren<ParticleSystemRenderer>();
            
            PrepareParticleSystem();
            
            _g.ShootPojectileAction += OnShootProjectileAction;
            
            _prepared = true;
        }

        private void PrepareParticleSystem()
        {
            var main = _ps.main;
            main.duration = 5;
            main.startSpeed = -4f;
            main.startLifetime = .25f;
            main.startSize = 0.1f;
            var emission = _ps.emission;
            emission.enabled = true;
            emission.rateOverTime = 100;
            var shape = _ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 1;
            shape.radiusThickness = 0;

            // this has purple debug particles... but I kinda like how it looks?
        }

        private void OnShootProjectileAction(GameObject o)
        {
            _hasFired = true;
            
            var phit = o.GetComponentInChildren<ProjectileHit>();
            var mult = Mathf.Lerp(1, _maxMultiplier, _timeWaited / InnerPeace.MaxDuration);
            phit.damage *= mult;
        }

        public void AddMultiplier(float f)
        {
            _maxMultiplier += f;
        }

        private void Update()
        {
            // _p.data.movement
            // var isMoving = _pa.Left.IsPressed || _pa.Right.IsPressed || _pa.Up.IsPressed || _pa.Down.IsPressed || _pa.Jump.IsPressed;
            var isMoving = ((Vector2)_pv.GetFieldValue("velocity")).magnitude > 0.5;
            var emission = _ps.emission;
            
            switch (_g.isReloading)
            {
                case false when !isMoving && !_hasFired && _timeWaited < InnerPeace.MaxDuration:
                    emission.enabled = true;
                    _timeWaited = Mathf.Clamp(_timeWaited + Time.deltaTime, 0f, InnerPeace.MaxDuration);
                    break;
                case true:
                    emission.enabled = false;
                    _hasFired = false;
                    _timeWaited = 0;
                    break;
                default:
                    emission.enabled = false;
                    break;
            }

            emission.rateOverTime = Mathf.Lerp(0, 100, _timeWaited / InnerPeace.MaxDuration);
        }

        public void EndGame()
        {
            _maxMultiplier = 0;
            _timeWaited = 0f;
            
            DestroyImmediate(_ps.gameObject);

            _g.ShootPojectileAction -= OnShootProjectileAction;
        }

        public void SetupRound()
        {
            _timeWaited = 0f;
        }

        public void CleanupRound()
        {
            _timeWaited = 0f;
        }
    }
}