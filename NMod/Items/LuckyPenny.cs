using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod.Items
{
    [NModItem(
          "Lucky Penny",
          "Gain gold when you enter a new stage",
          "Gain <style=cIsUtility>25</style> (+25 per stack) gold when you enter a new stage. Payout increases with stage number.")]
    class LuckyPenny : CustomItemBase
    {
        const uint MONEY_GAIN_BASE = 25;
        const uint MONEY_PER_STACK = 25;
        const float MULT_PER_STAGE = 1f;
        public static string Name => "luckypenny";
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier1;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Utility,
        };

        //TODO: Detect what stage number you're on
        public override void RegisterHooks(ItemIndex itemIndex)
        {

            On.RoR2.SceneDirector.PopulateScene += (orig, self) =>
            {
                foreach(var playerCharacter in PlayerCharacterMasterController.instances)
                {
                    if(playerCharacter && playerCharacter.master && playerCharacter.master.inventory && playerCharacter.master.inventory.GetItemCount(itemIndex) is int count && count != 0)
                    {
                        playerCharacter.master.GiveMoney((uint)(
                            (MONEY_GAIN_BASE + (count-1) * MONEY_PER_STACK) 
                            * (1 + (MULT_PER_STAGE * Run.instance.stageClearCount - 1))));
                    }
                }
                orig(self);
            };
        }
    }
}
