using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using VECsPlugin.Cards;
using VECsPlugin.Effects;
using VECsPlugin.Util;

namespace VECsPlugin
{
    [BepInPlugin("org.virepri.rounds.vecs", "Virepri's Extra Cards", "0.1")]
    [BepInProcess("Rounds.exe")]
    [BepInDependency("com.willis.rounds.unbound", "2.4.0")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    public class VECsPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            // Add cards
            CustomCard.BuildCard<PanicCard>();
            CustomCard.BuildCard<ExtraMag>();
            CustomCard.BuildCard<BiggerMag>();
            CustomCard.BuildCard<Teleporter>();

            // Register hooks for ReversibleEffects
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, handler => OnGameEnd(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, handler => OnRoundStart(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, handler => OnRoundEnd(handler));
        }

        public static List<ReversibleEffect> reversibleEffects
        {
            get
            {
                var result = new List<ReversibleEffect>();
                
                foreach (var instancePlayer in PlayerManager.instance.players)
                {
                    result.AddRange(instancePlayer.GetComponents<ReversibleEffect>());
                }
                
                return result;
            }
        }

        private IEnumerator OnGameEnd(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Game end");
            foreach (var reversibleEffect in reversibleEffects)
            {
                UnityEngine.Debug.Log($"Game end: {reversibleEffect.GetType().Name}");
                reversibleEffect.EndGame();

                if (reversibleEffect is MonoBehaviour)
                {
                    DestroyImmediate((MonoBehaviour) reversibleEffect);
                }
            }

            // Kill all reversible effects
            reversibleEffects.RemoveRange(0, reversibleEffects.Count);

            yield break;
        }

        private IEnumerator OnRoundStart(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Round start");
            foreach (var reversibleEffect in reversibleEffects)
            {
                UnityEngine.Debug.Log($"Round start: {reversibleEffect.GetType().Name}");
                reversibleEffect.SetupRound();
            }

            yield break;
        }

        private IEnumerator OnRoundEnd(IGameModeHandler handler)
        {
            UnityEngine.Debug.Log("Round end");
            foreach (var reversibleEffect in reversibleEffects)
            {
                UnityEngine.Debug.Log($"Round end: {reversibleEffect.GetType().Name}");
                reversibleEffect.CleanupRound();
            }

            yield break;
        }
    }
}