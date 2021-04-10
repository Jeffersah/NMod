using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod.Items
{
    [NModItem(
          "Adrenaline",
          "Deal more damage the lower your HP %",
          "Deal up to <style=cIsUtility>25%</style> (+12.5% per stack) more damage as your HP % decreases.",
          "Oh this? This is just some drugs.")]
    class DesperateThymes : CustomItemBase
    {
        const float DAMAGE_BASE = .25f;
        const float DAMAGE_STACK = 0.125f;
        const float MAX_HP = 0.9f;
        public static string Name => "desperatethymes";
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier2;

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
                            float dmg_mult = DAMAGE_BASE + DAMAGE_STACK * (itemCount - 1);
                            float hpPercent = Math.Min(MAX_HP, attackerBody.healthComponent.combinedHealthFraction);
                            hpPercent /= MAX_HP;
                            float add_mult = dmg_mult * (1 - hpPercent);
                            damageInfo.damage += damageInfo.damage * add_mult;
                        }
                    }
                }
                orig(self, damageInfo);
            };
        }
    }
}
