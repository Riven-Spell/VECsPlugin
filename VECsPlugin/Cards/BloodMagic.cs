using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using VECsPlugin.Effects;

namespace VECsPlugin.Cards
{
    public class BloodMagic : CustomCard
    {
        protected override string GetTitle()
        {
            return "Blood Magic";
        }

        protected override string GetDescription()
        {
            return "Block outside of your cooldown, for a price.\n<i>A wound is cheaper than being dead.</i>";
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
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
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
            var hook = player.gameObject.GetOrAddComponent<FailTryBlockHookEffect>();
            hook.OnFailBlockAction += b =>
            {
                // data.health -= data.maxHealth / 10;
                data.healthHandler.TakeDamage(Vector2.up * (data.maxHealth / 5), Vector2.zero, Color.red, ignoreBlock: true);
                b.RPCA_DoBlock(false, true);
            };
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}