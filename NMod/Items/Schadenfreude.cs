using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod.Items
{
    [NModItem(
          "Schadenfreude",
          "Increase damage the lower the targets HP %",
          "Deal up to <style=cIsUtility>25%</style> (+25% per stack) more damage to enemies at low health. Scales as target health decreases.")]
    class Schadenfreude : CustomItemBase
    {
        const float HP_CEILING = 0.9f;
        const float DMG_BUFF = 0.25f;
        const float DMG_BUFF_PER = 0.25f;

        public static string Name => nameof(Schadenfreude).ToLower();
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier3;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Damage,
        };

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                if (damageInfo.attacker)
                {
                    var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (attackerBody != null && attackerBody && attackerBody.master && attackerBody.master.inventory)
                    {
                        int itemCount = attackerBody.master.inventory.GetItemCount(itemIndex);
                        if (itemCount > 0)
                        {
                            float dmgBoostPercent = 1 - (Math.Min(HP_CEILING, self.combinedHealthFraction) / HP_CEILING);
                            damageInfo.damage *= 1 + dmgBoostPercent * StackUtils.LinearStack(itemCount, DMG_BUFF, DMG_BUFF_PER);
                        }
                    }
                }
                orig(self, damageInfo);
            };
        }
    }
}
