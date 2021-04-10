using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod.Items
{
    [NModItem(
        "Panic Necklace",
        "Move faster after taking damage",
        "Increases your movement speed by <style=cIsUtility>20%</style> (+20% per stack) for <style=cIsUtility>5 seconds</style> after you take damage",
        "Found in an all but abandoned wooden chest on a decrepit world. With it was a note, which read simply: \"The Eye of Chithulu is coming. Do Not Panic.\"")]
    class PanicNecklace : CustomItemBase
    {
        const float BASE_TIME = 5;
        const float TIME_PER_STACK = 0;

        const float BASE_AMT = 0.2f;
        const float AMT_PER_STACK = 0.2f;

        public static string Name => "panicnecklace";
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier1;

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                if (damageInfo.attacker)
                {
                    if (self.body != null && self.body && self.body.master && self.body.master.inventory)
                    {
                        int itemCount = self.body.master.inventory.GetItemCount(itemIndex);
                        if (itemCount > 0)
                        {
                            var buff = NModLoader.LoadedCustomBuffs[Buffs.PanicNecklace.Name].Index;
                            if (!self.body.HasBuff(buff))
                            {
                                var mult = BASE_AMT + AMT_PER_STACK * (itemCount - 1);
                                Buffs.PanicNecklace.SetCharacterMoveSpeedAdjust(self.body, self.body.baseMoveSpeed * mult);
                                self.body.AddTimedBuff(buff, BASE_TIME + TIME_PER_STACK * (itemCount - 1));
                            }
                        }
                    }
                }
                orig(self, damageInfo);
            };
        }
    }
}
