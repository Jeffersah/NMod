using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace NMod.Items
{
    [NModItem(
          "Envy",
          "Deal more damage to elites, but less to bosses",
          "Deal 50% (+50% per stack) more damage to elites, but deal 50% (-50% per stack) as much damage to bosses.")]
    class Envy : CustomItemBase
    {
        const float BOSS_DMG_BASE = 0.5f;
        const float BOSS_DMG_STACK = 0.5f;

        const float ELITE_DMG_BASE = 0.5f;
        const float ELITE_DMG_STACK = 0.5f;


        public static string Name => nameof(Envy).ToLower();
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Lunar;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.AIBlacklist,
            ItemTag.Cleansable,
        };

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                if ((self.body.isElite || self.body.isBoss) && damageInfo.attacker)
                {
                    var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (attackerBody != null && attackerBody && attackerBody.master && attackerBody.master.inventory)
                    {
                        int itemCount = attackerBody.master.inventory.GetItemCount(itemIndex);
                        if (itemCount > 0)
                        {
                            if(self.body.isElite)
                            {
                                damageInfo.damage *= 1 + StackUtils.LinearStack(itemCount, ELITE_DMG_BASE, ELITE_DMG_STACK);
                            }
                            else if(self.body.isBoss)
                            {
                                damageInfo.damage *= StackUtils.ExponentialStack(itemCount, BOSS_DMG_BASE, BOSS_DMG_STACK);
                            }
                        }
                    }
                }
                orig(self, damageInfo);
            };
        }
    }
}
