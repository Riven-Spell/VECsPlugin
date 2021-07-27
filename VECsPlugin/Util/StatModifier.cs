using System;
using System.Collections.Generic;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Util
{

    public class FloatStatModifier {
        public float Additive;
        public float Multiplicative = 1; // Applied first
        
        public float ApplyStat(float baseT)
        {
            return (baseT * Multiplicative) + Additive;
        }
    }

    public class FloatStatManager : MonoBehaviour, ReversibleEffect
    {
        private List<FloatStatModifier> mods = new List<FloatStatModifier>();
        private bool basePrepared;
        private float baseStat;
        private float baseStatOffset;

        public string PropertyName;
        public Action<float> AdjustTarget;
        public Func<float> GetTarget;
        
        public void EndGame()
        {
            Destroy(this);
        }

        public void SetupRound()
        {
            baseStat = GetTarget();
            baseStatOffset = 0;
            Update();
        }

        public void CleanupRound()
        {
            AdjustTarget(-baseStatOffset );
        }

        public void RegisterModifier(FloatStatModifier mod)
        {
            mods.Add(mod);
        }

        public void RemoveModifier(FloatStatModifier mod)
        {
            mods.Remove(mod);
        }

        public List<FloatStatModifier> GetModifiers()
        {
            return mods;
        }

        public float GetBase()
        {
            if (!basePrepared)
            {
                baseStat = GetTarget();
                basePrepared = true;
            }
            
            return baseStat;
        }

        public void Update()
        {
            var result = GetBase();

            foreach (var statModifier in mods)
            {
                result = statModifier.ApplyStat(result);
            }

            AdjustTarget(-baseStatOffset); // Return to the original base
            baseStatOffset = result - baseStat;
            AdjustTarget(baseStatOffset);
            UnityEngine.Debug.Log($"{PropertyName}: {GetTarget()}");
        }
    }

    public class StatModifierRegistry : MonoBehaviour
    {
        public const string GunReloadSpeedMultiplier = "GunReloadSpeedMultiplier";
        public const string GunFireRate = "GunFireRate";
        public const string GunSpread = "GunSpread"; 
        
        private Dictionary<string, FloatStatManager> FloatStatManagers = new Dictionary<string, FloatStatManager>();

        public FloatStatManager GetOrAddFloatStatManager(string name, Func<FloatStatManager> init)
        {
            if (!FloatStatManagers.ContainsKey(name))
            {
                FloatStatManagers.Add(name, init());                
            }

            return FloatStatManagers[name];
        }
    }

    public class FloatStatManagerInit
    {
        public static Func<FloatStatManager> PrepareInitReloadSpeed(GameObject character, Gun gun)
        {
            return () =>
            {
                var newManager = character.AddComponent<FloatStatManager>();
                newManager.GetTarget = () => gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier;
                newManager.AdjustTarget = f => { gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier += f; };
                newManager.PropertyName = "Reload speed";
                VECsPlugin.reversibleEffects.Add(newManager);
                return newManager;
            };
        }
        
        public static Func<FloatStatManager> PrepareInitAttackSpeed(GameObject character, Gun gun)
        {
            return () =>
            {
                var newManager = character.AddComponent<FloatStatManager>();
                newManager.GetTarget = () => gun.attackSpeed;
                newManager.AdjustTarget = f => { gun.attackSpeed += f; };
                newManager.PropertyName = "Attack speed";
                VECsPlugin.reversibleEffects.Add(newManager);
                return newManager;
            };
        }
        
        public static Func<FloatStatManager> PrepareInitAttackSpread(GameObject character, Gun gun)
        {
            return () =>
            {
                var newManager = character.AddComponent<FloatStatManager>();
                newManager.GetTarget = () => gun.spread;
                newManager.AdjustTarget = f => { gun.spread += f; };
                newManager.PropertyName = "Attack Spread";
                VECsPlugin.reversibleEffects.Add(newManager);
                return newManager;
            };
        }
    }
}