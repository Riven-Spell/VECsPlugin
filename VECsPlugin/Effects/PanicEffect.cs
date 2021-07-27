using System;
using System.Collections.Generic;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VECsPlugin.Util;

namespace VECsPlugin.Effects
{
    public class PanicEffect : MonoBehaviour, ReversibleEffect
    {
        private Player m_player;
        private FloatStatModifier m_Reload;
        private FloatStatModifier m_AttackSpeed;
        private FloatStatModifier m_Spread;
        private List<FloatStatManager> m_toUpdate;
        private bool m_prepareOnce;

        public void PrepareOnce(Player p, CharacterStatModifiers mods, FloatStatModifier Reload, FloatStatModifier AttackSpeed, FloatStatModifier Spread, List<FloatStatManager> managers)
        {
            if (m_prepareOnce)
                return;
            
            m_player = p;
            m_Reload = Reload;
            m_AttackSpeed = AttackSpeed;
            m_Spread = Spread;
            m_toUpdate = managers;

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

        private void DoModifierUpdate()
        {
            foreach (var floatStatManager in m_toUpdate)
            {
                floatStatManager.Update();
            }
        }

        
        private void OnDealtDamage(Vector2 dmg, bool selfDamage)
        {
            var perc = m_player.data.HealthPercentage;

            // m_Reload.Multiplicative = 1 / (perc * m_cardMultiplier);
            // m_AttackSpeed.Multiplicative = 1 / (perc * m_cardMultiplier);
            // m_Spread.Multiplicative = perc * m_cardMultiplier * 0.6f;
            m_Reload.Multiplicative = Mathf.Lerp(1 / m_cardMultiplier, 1, perc);
            m_AttackSpeed.Multiplicative = Mathf.Lerp(1 / m_cardMultiplier, 1, perc);
            m_Spread.Additive = Mathf.Lerp(0, .04f * m_cardMultiplier, 1 - perc);
            DoModifierUpdate();
        }

        public void EndGame()
        {
            m_Reload.Multiplicative = 1f;
            m_AttackSpeed.Multiplicative = 1f;
            m_Spread.Additive = 0f;
            DoModifierUpdate();
            Destroy(this);     
        }

        public void SetupRound()
        {
            m_Reload.Multiplicative = 1f;
            m_AttackSpeed.Multiplicative = 1f;
            m_Spread.Multiplicative = 1f;
            DoModifierUpdate();
        }

        public void CleanupRound()
        {
            m_Reload.Multiplicative = 1f;
            m_AttackSpeed.Multiplicative = 1f;
            m_Spread.Multiplicative = 1f;
            DoModifierUpdate();
        }
    }
}