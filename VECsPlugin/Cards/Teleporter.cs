using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;
using VECsPlugin.Effects.Bullet;

namespace VECsPlugin.Cards
{
    public class Teleporter : CustomCard
    {
        protected override string GetTitle()
        {
            return "Teleporter";
        }

        protected override string GetDescription()
        {
            return "Your next shot after you block teleports you wherever it hits (Yes, off the screen, too)";
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
            return null;
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
            var thisTeleporterEffect = player.gameObject.GetOrAddComponent<TeleporterEffect>();
            thisTeleporterEffect.PrepareOnce(player, gun);
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}