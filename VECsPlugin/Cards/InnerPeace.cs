using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class InnerPeace : CustomCard
    {
        public const float MaxDuration = 10f;
        public const float MaxMultiplier = 5f;
        
        protected override string GetTitle()
        {
            return "Inner Peace";
        }

        protected override string GetDescription()
        {
            return $"Standing still for up to {MaxDuration} seconds <color=#00FF00>raises</color> the damage of your next magazine. A full magazine <b>must</b> be expended to charge after the first shot.\n<i>The secret to true power is how calm you can be under pressure.</i>";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]{};
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            statModifiers.automaticReload = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            var thisInnerPeaceEffect = player.gameObject.GetOrAddComponent<InnerPeaceEffect>();
            thisInnerPeaceEffect.PrepareOnce(player, data.playerActions, gun);
            thisInnerPeaceEffect.AddMultiplier(MaxMultiplier);
            
            characterStats.automaticReload = false;
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}