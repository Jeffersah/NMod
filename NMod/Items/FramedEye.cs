using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using static RoR2.CharacterBody;

namespace NMod.Items
{
    [NModItem(
          "Framed Eye",
          "Become invincible briefly after taking damage",
          "Become invincible for <style=cIsUtility>2</style> (+1 per stack) seconds after taking damage. Cooldown for 3 (-10% per stack) seconds afterwards.")]
    class FramedEye : CustomBehaviorItemBase<FramedEye.Behavior>
    {
        const float INVINCIBILITY_BASE = 2;
        const float INVINCIBILITY_STACK = 1;

        public const float COOLDOWN_BASE = 3;
        public const float COOLDOWN_REDUCE = 0.9f;

        public static string Name => nameof(FramedEye).ToLower();
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier3;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Utility
        };

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, dmg) =>
            {
                dmg.rejected |= self.body?.GetComponent<Behavior>()?.OnTakeDamage() ?? false;
                orig(self, dmg);
            };
            base.RegisterHooks(itemIndex);
        }

        public class Behavior: ItemBehavior
        {
            public bool OnTakeDamage()
            {
                if(body.HasBuff(NModLoader.LoadedCustomBuffs[Buffs.FramedEyeImmortality.Name].Index))
                {
                    return true;
                }
                else if(body.HasBuff(NModLoader.LoadedCustomBuffs[Buffs.FramedEyeCooldown.Name].Index))
                {
                    return false;
                }
                body.AddTimedBuff(NModLoader.LoadedCustomBuffs[Buffs.FramedEyeImmortality.Name].Index, StackUtils.LinearStack(stack, INVINCIBILITY_BASE, INVINCIBILITY_STACK));
                return false;
            }
        }
    }
}
