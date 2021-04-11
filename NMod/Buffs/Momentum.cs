using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using R2API.Networking;

namespace NMod.Buffs
{
    [NModBuff]
    class Momentum : CustomBuffBase
    {
        public static string Name = "momentum_buff";
        public override string InternalName => Name;
        public override string AssetPath => "momentum/momentum_icon.png";
        public override bool IsDebuff => false;
        public override bool CanStack => true;

        public override void RegisterHooks(BuffIndex buffIndex)
        {
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                orig(self);
                var buffCount = self.GetBuffCount(buffIndex);
                if (buffCount > 0)
                {
                    var hpBonus = StackUtils.LinearStack(buffCount, Items.Momentum.BASE_HEALTH_BONUS, Items.Momentum.STACK_HEALTH_BONUS);
                    typeof(CharacterBody).GetProperty("maxHealth").SetValue(self, self.maxHealth * (1 + hpBonus));
                }
            };
        }
    }
}
