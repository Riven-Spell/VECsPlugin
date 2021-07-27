using System;
using UnboundLib.GameModes;

namespace VECsPlugin.Effects
{
    public class ExtraMagEffect : ReversibleEffect
    {
        private Gun m_Gun;
        private bool m_prepared;
        private bool m_secondMagLoaded = true;
        
        public void PrepareOnce(Gun g)
        {
            if (m_prepared)
                return;
            
            m_Gun = g;

            m_prepared = true;

            if (GameModeManager.CurrentHandler.Name == "Sandbox")
                m_Gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier /= 2;
        }

        private bool m_lastReloadState = false;
        
        public void Update()
        {
            if (m_Gun.isReloading && m_Gun.isReloading != m_lastReloadState)
            {
                // Spend the mag
                m_secondMagLoaded = !m_secondMagLoaded;
                
                // Adjust our reload speed
                var ammo = m_Gun.GetComponentInChildren<GunAmmo>();
                // Alternate
                ammo.reloadTimeMultiplier *= m_secondMagLoaded ? .25f : 4f;
                UnityEngine.Debug.Log($"Current fire rate: {ammo.reloadTimeMultiplier}");
            }
            
            m_lastReloadState = m_Gun.isReloading;
        }

        public override void EndGame()
        {
            VECsPlugin.reversibleEffects.Remove(this);
            Destroy(this);
        }

        public override void SetupRound()
        {
            m_secondMagLoaded = true;
            var ammo = m_Gun.GetComponentInChildren<GunAmmo>();
            UnityEngine.Debug.Log($"Current fire rate: {ammo.reloadTimeMultiplier}");
            ammo.reloadTimeMultiplier /= 2;
            UnityEngine.Debug.Log($"Current fire rate: {ammo.reloadTimeMultiplier}");
        }

        public override void CleanupRound()
        {
            var ammo = m_Gun.GetComponentInChildren<GunAmmo>();
            ammo.reloadTimeMultiplier *= m_secondMagLoaded ? .5f : 2f;
            UnityEngine.Debug.Log($"Current fire rate: {ammo.reloadTimeMultiplier}");
        }
    }
}