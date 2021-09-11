using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class Capture : CustomCard
    {
        protected override string GetTitle()
        {
            return "Capture";
        }

        protected override string GetDescription()
        {
            return "Blocking immediately after your last shot hits an enemy turns your next shot into a teleporter targeting them.\n<i>GOTCHA!</i>";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[] { };
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
            cardInfo.categories = new[] { Categories.Teleporter };
            cardInfo.blacklistedCategories = new[] { Categories.Teleporter };
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            var thisCaptureEffect = player.gameObject.GetOrAddComponent<CaptureEffect>();
            thisCaptureEffect.PrepareOnce(player, gun);
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}