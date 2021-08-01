using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;
using VECsPlugin.Util;

namespace VECsPlugin.Cards
{
    public class ExtraMag : CustomCard
    {
        protected override string GetTitle()
        {
            return "Extra Magazine";
        }

        protected override string GetDescription()
        {
            return "Magazines alternate between reloading <color=#FFFF00>Twice</color> and <color=#FFFF00>Half</color> as fast.";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[] { };
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

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            statModifiers.automaticReload = false;
            cardInfo.allowMultiple = false;
            // cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Temporary") };
            // cardInfo.blacklistedCategories = new CardCategory[] {CustomCardCategories.instance.CardCategory("Temporary")};
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            characterStats.automaticReload = false; // Auto reload is disabled, because it creates a bottomless clip.

            GameObject o;
            var thisStatModRegistry = (o = player.gameObject).GetOrAddComponent<StatModifierRegistry>();
            var thisReloadSpeedManager = thisStatModRegistry.GetOrAddFloatStatManager(StatModifierRegistry.GunReloadSpeedMultiplier, FloatStatManagerInit.PrepareInitReloadSpeed(o, gun));
            var thisReloadSpeedStatModifier = new FloatStatModifier();
            thisReloadSpeedManager.RegisterModifier(thisReloadSpeedStatModifier);
            var thisExtraMagEffect = o.GetOrAddComponent<ExtraMagEffect>();
            thisExtraMagEffect.isActive = true; // Turn off the kill switch
            thisExtraMagEffect.PrepareOnce(gun, thisReloadSpeedStatModifier, thisReloadSpeedManager);
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}