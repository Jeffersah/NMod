using BepInEx.Logging;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RoR2.CharacterBody;

namespace NMod.Items
{
    abstract class CustomBehaviorItemBase<TBehavior>: CustomItemBase
        where TBehavior: ItemBehavior
    {
        private CharacterStorage<int> PreviousItemCount { get; } = new CharacterStorage<int>();
        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                var oldValue = PreviousItemCount.TryGetValue(self, out int oldv) ? oldv : 0;
                var currentCount = self.inventory.GetItemCount(itemIndex);
                if (oldValue != currentCount)
                {
                    self.AddItemBehavior<TBehavior>(currentCount);
                }
                orig(self);
            };
        }
    }
}
