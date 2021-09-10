using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class GravityWells : CustomCard
    {
        public const float GravityWellTime = 4f;
        public const float GravityWellRadius = 3.5f;
        public const float GravityWellForce = 250f;
        
        protected override string GetTitle()
        {
            return "Gravity Wells";
        }

        protected override string GetDescription()
        {
            return $"Upon impact, your bullets create a gravity well that lasts for {GravityWellTime} seconds.\n<i>Sometimes, you just need to be a greater danger to yourself than your opponent.</i>";
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
            gun.projectileColor = Color.magenta;

            var thisGravityWellEffect = player.gameObject.GetOrAddComponent<GravityWellEffect>();
            thisGravityWellEffect.PrepareOnce(gun);
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