using System.Collections.Generic;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;
using VECsPlugin.Util;

namespace VECsPlugin.Cards
{
    public class PanicCard : CustomCard
    {
        protected override string GetTitle()
        {
            return "Panic!";
        }

        protected override string GetDescription()
        {
            return @"As <color=#00FF00>health</color> gets lower, <color=#FFFF00>attack/reload</color> speed <color=#00FF00>drastically increase</color>, with </color=#FF0000>reduced</color> accuracy";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]{};
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
            CharacterStatModifiers statModifiers)
        {
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Temporary") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Temporary") };
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            var thisStatModRegistry = player.gameObject.GetOrAddComponent<StatModifierRegistry>();
            var thisReloadSpeedManager = thisStatModRegistry.GetOrAddFloatStatManager(StatModifierRegistry.GunReloadSpeedMultiplier, FloatStatManagerInit.PrepareInitReloadSpeed(gameObject, gun));
            var thisAttackSpeedManager = thisStatModRegistry.GetOrAddFloatStatManager(StatModifierRegistry.GunFireRate, FloatStatManagerInit.PrepareInitAttackSpeed(gameObject, gun));
            var thisSpreadManager = thisStatModRegistry.GetOrAddFloatStatManager(StatModifierRegistry.GunSpread, FloatStatManagerInit.PrepareInitAttackSpread(gameObject, gun));

            var ReloadModifier = new FloatStatModifier();
            thisReloadSpeedManager.RegisterModifier(ReloadModifier);
            var AttackSpeedModifier = new FloatStatModifier();
            thisAttackSpeedManager.RegisterModifier(AttackSpeedModifier);
            var SpreadModifier = new FloatStatModifier();
            thisSpreadManager.RegisterModifier(SpreadModifier);
            
            
            var thisPanicEffect = player.gameObject.GetOrAddComponent<PanicEffect>();
            thisPanicEffect.PrepareOnce(player, characterStats, ReloadModifier, AttackSpeedModifier, SpreadModifier, new List<FloatStatManager>() {thisReloadSpeedManager, thisAttackSpeedManager, thisSpreadManager});
            thisPanicEffect.IncreaseMultiplier(4f);
            VECsPlugin.reversibleEffects.Add(thisPanicEffect);
        }

        public override void OnRemoveCard() { }
    }
}