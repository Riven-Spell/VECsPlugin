using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using VECsPlugin.Cards;
using VECsPlugin.Effects;
using VECsPlugin.Util;

namespace VECsPlugin
{
    [BepInPlugin("org.virepri.rounds.vecs", "Virepri's Extra Cards", "1.1.0")]
    [BepInProcess("Rounds.exe")]
    [BepInDependency("com.willis.rounds.unbound", "2.4.0")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    public class VECsPlugin : BaseUnityPlugin
    {
        public const string ModName = "VECs 1.0";
        
        private void Awake()
        {
            // Have harmony patch things
            new Harmony("org.virepri.rounds.vecs").PatchAll();
        }
        
        private void Start()
        {
            // Add cards
            CustomCard.BuildCard<PanicCard>();
            CustomCard.BuildCard<ExtraMag>();
            CustomCard.BuildCard<BiggerMag>();
            CustomCard.BuildCard<Teleporter>();
            CustomCard.BuildCard<BloodMagic>();
            CustomCard.BuildCard<Consume>();
            CustomCard.BuildCard<InnerPeace>();
            CustomCard.BuildCard<Tennis>();
            CustomCard.BuildCard<MagOfHolding>();
            CustomCard.BuildCard<GravityWells>();
            CustomCard.BuildCard<AntiGravityWells>();

            // Register hooks for ReversibleEffects
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, handler => OnGameEnd(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, handler => OnRoundStart(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, handler => OnRoundEnd(handler));
        }

        public static List<RoundTemporaryEffect> RoundTemporaryEffects
        {
            get
            {
                var result = new List<RoundTemporaryEffect>();

                foreach (var instancePlayer in PlayerManager.instance.players)
                {
                    result.AddRange(instancePlayer.GetComponents<RoundTemporaryEffect>());
                }
                
                return result;
            }
        }
        
        public static List<GameTemporaryEffect> GameTemporaryEffects
        {
            get
            {
                var result = new List<GameTemporaryEffect>();
                
                foreach (var instancePlayer in PlayerManager.instance.players)
                {
                    result.AddRange(instancePlayer.GetComponents<GameTemporaryEffect>());
                }
                
                return result;
            }
        }

        private IEnumerator OnGameEnd(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Game end");
            foreach (var reversibleEffect in GameTemporaryEffects)
            {
                UnityEngine.Debug.Log($"Game end: {reversibleEffect.GetType().Name}");
                reversibleEffect.EndGame();

                if (reversibleEffect is MonoBehaviour)
                {
                    DestroyImmediate((MonoBehaviour) reversibleEffect);
                }
            }

            // Kill all reversible effects
            RoundTemporaryEffects.RemoveRange(0, RoundTemporaryEffects.Count);

            yield break;
        }

        private IEnumerator OnRoundStart(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Round start");
            foreach (var reversibleEffect in RoundTemporaryEffects)
            {
                UnityEngine.Debug.Log($"Round start: {reversibleEffect.GetType().Name}");
                reversibleEffect.SetupRound();
            }

            yield break;
        }

        private IEnumerator OnRoundEnd(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Round end");
            foreach (var reversibleEffect in RoundTemporaryEffects)
            {
                UnityEngine.Debug.Log($"Round end: {reversibleEffect.GetType().Name}");
                reversibleEffect.CleanupRound();
            }

            yield break;
        }
    }
}