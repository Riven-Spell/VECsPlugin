﻿using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using VECsPlugin.Cards;
using VECsPlugin.Effects;

namespace VECsPlugin
{
    [BepInPlugin("org.virepri.rounds.vecs", "Panic Card", "0.1")]
    [BepInProcess("Rounds.exe")]
    [BepInDependency("com.willis.rounds.unbound", "2.3.0")]
    public class VECsPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            // Add cards
            CustomCard.BuildCard<PanicCard>();
            CustomCard.BuildCard<ExtraMag>();
            
            // Register hooks for ReversibleEffects
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, handler => OnGameEnd(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, handler => OnRoundStart(handler));
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, handler => OnRoundEnd(handler));
        }

        public static List<ReversibleEffect> reversibleEffects = new List<ReversibleEffect>();

        private IEnumerator OnGameEnd(IGameModeHandler handler)
        {
            foreach (var reversibleEffect in reversibleEffects)
            {
                reversibleEffect.EndGame();
            }

            yield break;
        }

        private IEnumerator OnRoundStart(IGameModeHandler handler)
        {
            foreach (var reversibleEffect in reversibleEffects)
            {
                reversibleEffect.SetupRound();
            }

            yield break;
        }

        private IEnumerator OnRoundEnd(IGameModeHandler handler)
        {
            foreach (var reversibleEffect in reversibleEffects)
            {
                reversibleEffect.CleanupRound();
            }
            
            yield break;
        }
    }
}