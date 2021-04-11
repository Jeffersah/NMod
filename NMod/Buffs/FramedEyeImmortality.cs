using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RoR2.CharacterBody;

namespace NMod.Buffs
{
    [NModBuff]
    class FramedEyeImmortality : CustomBuffBase
    {
        public static string Name = "framedeye_buff";
        public override string InternalName => Name;
        public override string AssetPath => "framedeye/framedeye_icon.png";

        public override bool IsDebuff => false;
        public override bool CanStack => false;

        public override void RegisterHooks(BuffIndex itemIndex)
        {
            On.RoR2.CharacterBody.OnBuffFinalStackLost += (orig, self, buff) =>
            {
                if(buff.buffIndex == itemIndex)
                {
                    self.AddTimedBuff(
                        NModLoader.LoadedCustomBuffs[FramedEyeCooldown.Name].Index, 
                        StackUtils.ExponentialStack(
                            self.inventory.GetItemCount(NModLoader.LoadedCustomItems[Items.FramedEye.Name].Index), 
                            Items.FramedEye.COOLDOWN_BASE, 
                            Items.FramedEye.COOLDOWN_REDUCE));
                }
                orig(self, buff);
            };
        }
    }

    [NModBuff]
    class FramedEyeCooldown : CustomBuffBase
    {
        public static string Name = "framedeye_cooldownbuff";
        public override string InternalName => Name;
        public override string AssetPath => "framedeye/framedeye_icon.png";
        public override bool IsDebuff => true;
        public override bool CanStack => false;
        public override Color BuffColor => new Color(0.2f, 0.2f, 0.2f);
    }
}
