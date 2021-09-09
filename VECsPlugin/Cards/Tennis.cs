using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class Tennis : CustomCard
    {
        protected override string GetTitle()
        {
            return "Tennis";
        }

        protected override string GetDescription()
        {
            return "Catch bullets out of the air by blocking and combine them with your next shot, increasing damage.";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]{};
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            return new GameObject();
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            var thisTennisEffect = player.gameObject.GetOrAddComponent<TennisEffect>();
            thisTennisEffect.PrepareOnce(player, gun);
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}