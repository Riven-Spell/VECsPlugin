using System;
using UnboundLib.GameModes;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public class PanicEffect : ReversibleEffect
    {
        private Player m_player;
        private Gun m_Gun;
        private bool m_prepareOnce;

        public void PrepareOnce(Player p, Gun g, CharacterStatModifiers mods)
        {
            if (m_prepareOnce)
                return;
            
            m_player = p;
            m_Gun = g;

            mods.WasDealtDamageAction += OnDealtDamage;
            
            
            m_prepareOnce = true;
        }

        private float m_cardMultiplier = 0f;

        public void IncreaseMultiplier(float amount)
        {
            m_cardMultiplier += amount;
            OnDealtDamage(Vector2.zero, false);
        }

        private float m_lastHealth;
        public void Update()
        {
            // If healed, trigger our update
            if (m_player.data.health > m_lastHealth)
            {
                OnDealtDamage(Vector2.zero, false);                
            }
            
            m_lastHealth = m_player.data.health;
        }

        private struct Alterations
        {
            public float reloadTimeMultiplier;
            public float attackSpeed;
            public float spread;

            public Alterations GetDifference(Alterations from)
            {
                return new Alterations
                {
                    reloadTimeMultiplier = reloadTimeMultiplier - from.reloadTimeMultiplier,
                    attackSpeed = attackSpeed - from.attackSpeed,
                    spread = spread - from.spread
                };
            }

            public Alterations Invert()
            {
                return new Alterations
                {
                    attackSpeed = -attackSpeed,
                    reloadTimeMultiplier = -reloadTimeMultiplier,
                    spread = -spread
                };
            }

            public void Apply(Gun gun)
            {
                gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier += reloadTimeMultiplier;
                gun.attackSpeed += attackSpeed;
                gun.spread += spread;
            }
        }
        
        private Alterations m_lastAlterations = new Alterations{};

        private Alterations GetPresent()
        {
            return new Alterations()
            {
                reloadTimeMultiplier = m_Gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier - m_lastAlterations.reloadTimeMultiplier,
                attackSpeed = m_Gun.attackSpeed - m_lastAlterations.attackSpeed,
                spread = m_Gun.spread - m_lastAlterations.spread,
            };
        }
        
        private Alterations GetAlterations()
        {
            var perc = m_player.data.HealthPercentage;

            var m_Base = GetPresent();
            
            return new Alterations
            {
                reloadTimeMultiplier = Mathf.Lerp(
                        m_Base.reloadTimeMultiplier / (m_cardMultiplier / 1.5f),
                        m_Base.reloadTimeMultiplier,
                        perc
                    ),
                attackSpeed = Mathf.Lerp(
                    m_Base.attackSpeed / m_cardMultiplier,
                    m_Base.attackSpeed,
                    perc
                    ),
                spread = m_Base.spread + ((1 - perc) * m_cardMultiplier * .06f)
            }.GetDifference(m_Base);
        }

        private void OnDealtDamage(Vector2 dmg, bool selfDamage)
        {
            var temp = GetAlterations();
            m_lastAlterations.Invert().Apply(m_Gun); // Undo the last alterations
            temp.Apply(m_Gun);
            m_lastAlterations = temp;

            var present = GetPresent();

            UnityEngine.Debug.Log($"insanity check: {present.reloadTimeMultiplier} {present.attackSpeed} {present.spread}");
            UnityEngine.Debug.Log($"Current gun stats: health: {m_player.data.HealthPercentage} multiplier: {(1f - m_player.data.HealthPercentage) * m_cardMultiplier} reload: {m_Gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier} attack speed: {m_Gun.attackSpeed} spread: {m_Gun.spread}");
            UnityEngine.Debug.Log($"Bases: multiplier: {m_cardMultiplier} reload: {m_lastAlterations.reloadTimeMultiplier} attack speed: {m_lastAlterations.attackSpeed} spread: {m_lastAlterations.spread}");
        }

        public override void EndGame()
        {
            VECsPlugin.reversibleEffects.Remove(this);
            Destroy(this);            
        }

        public override void SetupRound()
        {
        }

        public override void CleanupRound()
        {
            m_lastAlterations.Invert().Apply(m_Gun);
        }
    }
}