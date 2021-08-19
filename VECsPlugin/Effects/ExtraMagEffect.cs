using System;
using UnboundLib.GameModes;
using UnityEngine;
using VECsPlugin.Util;

namespace VECsPlugin.Effects
{
    public class ExtraMagEffect : MonoBehaviour, RoundTemporaryEffect
    {
        private Gun m_Gun;
        private FloatStatModifier m_GunSpeedMod;
        private FloatStatManager m_fsm;
        private bool m_prepared;
        private bool m_secondMagLoaded = true;

        public bool isActive;
        
        public void PrepareOnce(Gun gun, FloatStatModifier gunSpeedMod, FloatStatManager fsm)
        {
            if (m_prepared)
                return;

            m_GunSpeedMod = gunSpeedMod;
            m_Gun = gun;
            m_fsm = fsm;

            if (GameModeManager.CurrentHandler.Name == "Sandbox")
            {
                m_GunSpeedMod.Multiplicative = .5f;
                m_fsm.UpdateSM();
            }

            m_prepared = true;
        }

        private bool m_lastReloadState = false;
        
        public void Update()
        {
            if (!isActive)
                return;
            
            if (!m_Gun.isReloading && m_Gun.isReloading != m_lastReloadState)
            {
                // Spend the mag
                m_secondMagLoaded = !m_secondMagLoaded;
                
                // Adjust our reload speed
                m_GunSpeedMod.Multiplicative = m_secondMagLoaded ? .5f : 2f;
                // fire the update
                m_fsm.UpdateSM();
            }
            
            m_lastReloadState = m_Gun.isReloading;
        }

        public void EndGame()
        {
            m_GunSpeedMod.Multiplicative = 1f;
            m_fsm.UpdateSM();
            isActive = false;
        }

        public void SetupRound()
        {
            m_secondMagLoaded = true;
            m_GunSpeedMod.Multiplicative = .5f;
            m_fsm.UpdateSM();
        }

        public void CleanupRound()
        {
            m_GunSpeedMod.Multiplicative = 1f; // Reset the mag
            m_fsm.UpdateSM();
        }
    }
}