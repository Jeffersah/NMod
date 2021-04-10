using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod.Items
{
    [NModItem(
          "Snake Eyes",
          "Chance to instantly kill enemies",
          "<style=cIsUtility>1%</style> (+1% per stack) chance to instantly kill enemies when you deal more than <style=cIsUtility>10%</style> (-1% per stack) of their HP in one hit.",
          "The Eyes decide who lives and who dies")]
    class SnakeEyes : CustomItemBase
    {
        const float BASE_PERC = .01f;
        const float ADD_PERC = .01f;

        const float HP_LIMIT = 0.1f;
        const float HP_LIMIT_ADJ = 0.99f;

        public static string Name => nameof(SnakeEyes).ToLower();
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
                            float minDamageForProc = StackUtils.InverseHyperbolicStack(itemCount, HP_LIMIT, HP_LIMIT_ADJ) * self.body.maxHealth;
                            float instakillChance = StackUtils.HyperbolicStack(itemCount, BASE_PERC, ADD_PERC);
                            float roll = UnityEngine.Random.Range(0f, 1f);
                            if(roll < instakillChance && damageInfo.damage >= minDamageForProc)
                            {
                                self.body.master.TrueKill();
                                return;
                            }
                        }
                    }
                }
                orig(self, damageInfo);
            };
        }
    }
}
