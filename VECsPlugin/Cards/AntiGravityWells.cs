using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class AntiGravityWells : CustomCard
    {
        protected override string GetTitle()
        {
            return "Anti-Gravity Wells";
        }

        protected override string GetDescription()
        {
            return $"Upon impact, your bullets create a anti-gravity well forcing your opponents out that lasts for {GravityWells.GravityWellTime} seconds.\n<i>No sir/ma'am, not that way.</i>";
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
            return CardThemeColor.CardThemeColorType.MagicPink;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            gun.projectileColor = Color.red;

            var thisGravityWellEffect = player.gameObject.GetOrAddComponent<GravityWellEffect>();
            thisGravityWellEffect.PrepareOnce(gun, gunAmmo, true);
            thisGravityWellEffect.AddStack();
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