using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class Consume : CustomCard
    {
        protected override string GetTitle()
        {
            return "Consume";
        }

        protected override string GetDescription()
        {
            return $"Eat another player's bullet upon blocking it, recovering {ConsumeEffect.ConsumeIncreaseDegree * 100f}% of the damage as HP.\n<i>Taking eating bullets to a whole new level. Yum!</i>";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[] { };
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.categories = new CardCategory[]
            {
                Categories.HealthRelatedBlock,
            };
            cardInfo.blacklistedCategories = new CardCategory[]
            {
                Categories.HealthRelatedBlock,
            };
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            var thisConsumeEffect = player.gameObject.GetOrAddComponent<ConsumeEffect>();
            thisConsumeEffect.IncreaseConsumeDegree(ConsumeEffect.ConsumeIncreaseDegree);
            thisConsumeEffect.PrepareOnce(data);
        }

        public override void OnRemoveCard()
        {
            
        }
        
        public override string GetModName()
        {
            return VECsPlugin.ModName;
        }
    }
}