using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class MagOfHolding : CustomCard
    {
        public const int AddedCapacity = 16;

        protected override string GetTitle()
        {
            return "Mag of Holding";
        }

        protected override string GetDescription()
        {
            return "<i>How in the world did they fit all of this ammo in here!?!?!?</i>";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat(){
                    amount = "+" + AddedCapacity,
                    positive = true,
                    stat = "Bullets"
                },
                new CardInfoStat()
                {
                    amount = "-1",
                    positive = false,
                    stat = "Mag capacity on full reload"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override GameObject GetCardArt()
        {
            return new GameObject();
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
            gunAmmo.maxAmmo += AddedCapacity;

            var thisMagOfHoldingEffect = player.gameObject.GetOrAddComponent<MagOfHoldingEffect>();
            thisMagOfHoldingEffect.PrepareOnce(player, gunAmmo, gun);
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}