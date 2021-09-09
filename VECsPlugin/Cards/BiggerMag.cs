using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib.Cards;
using UnityEngine;

namespace VECsPlugin.Cards
{
    public class BiggerMag : CustomCard
    {
        protected override string GetTitle()
        {
            return "Bigger Magazine";
        }

        protected override string GetDescription()
        {
            return "Double your mag size, Double your reload time.\n<i>Double the pain, Double the trouble.</i>";
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
            
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            gun.GetComponentInChildren<GunAmmo>().reloadTimeMultiplier *= 2;
            gun.GetComponentInChildren<GunAmmo>().maxAmmo *= 2;
            gun.ammo *= 2;
        }

        public override void OnRemoveCard()
        {
            // TODO
        }
    }
}