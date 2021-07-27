using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

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
            return CardInfo.Rarity.Uncommon;
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
            var thisPanicEffect = player.gameObject.GetOrAddComponent<PanicEffect>();
            thisPanicEffect.PrepareOnce(player, gun, characterStats);
            thisPanicEffect.IncreaseMultiplier(4f);
            VECsPlugin.reversibleEffects.Add(thisPanicEffect);
        }

        public override void OnRemoveCard() { }
    }
}