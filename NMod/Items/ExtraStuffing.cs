using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using static RoR2.CharacterBody;

namespace NMod.Items
{
    [NModItem(
          "Extra Stuffing",
          "Gain immunity to <style=cIsUtility>1</style> (+1 per stack) attack when you exit combat",
          "Gain immunity to <style=cIsUtility>1</style> (+1 per stack) attack when you exit combat")]
    class ExtraStuffing : CustomBehaviorItemBase<ExtraStuffing.Behavior>
    {
        const int TICKS_FOR_LEAVE_COMBAT = 60 * 10;

        public static string Name => "extrastuffing";
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier1;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Utility
        };

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            base.RegisterHooks(itemIndex);
            On.RoR2.HealthComponent.TakeDamage += (orig, self, dmg) =>
            {
                self.body.GetComponent<Behavior>()?.ResetTimer();
                dmg.attacker?.GetComponent<CharacterBody>()?.GetComponent<Behavior>()?.ResetTimerAndStacks();
                orig(self, dmg);
            };
        }

        public class Behavior: ItemBehavior
        {
            public int timeSinceCombat;
            public Behavior()
            {
                timeSinceCombat = 0;
            }

            public void ResetTimer()
            {
                timeSinceCombat = 0;
            }

            public void ResetTimerAndStacks()
            {
                ResetTimer();
                var buff = NModLoader.LoadedCustomBuffs[Buffs.ExtraStuffing.Name].Index;
                var buffCount = body.GetBuffCount(buff);
                while ((buffCount--) > 0)
                {
                    body.RemoveBuff(buff);
                }
            }

            public void Update()
            {
                timeSinceCombat++;
                if(timeSinceCombat >= TICKS_FOR_LEAVE_COMBAT)
                {
                    var buff = NModLoader.LoadedCustomBuffs[Buffs.ExtraStuffing.Name].Index;
                    var buffCount = body.GetBuffCount(buff);
                    while((buffCount++) < stack)
                    {
                        body.AddBuff(buff);
                    }
                }
            }
        }
    }
}
