using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NMod.Buffs
{
    [NModBuff]
    class ExtraStuffing : CustomBuffBase
    {

        public static string Name = "extrastuffing_buff";
        public override string InternalName => Name;
        public override string AssetPath => "extrastuffing/extrastuffing_icon.png";

        public override bool IsDebuff => false;
        public override bool CanStack => true;

        public override void RegisterHooks(BuffIndex buffIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damage) =>
            {
                if(self.body.GetBuffCount(buffIndex) != 0)
                {
                    self.body.RemoveBuff(buffIndex);
                    self.body.GetComponent<Items.ExtraStuffing.Behavior>().timeSinceCombat = 0;
                    damage.rejected = true;
                }
                orig(self, damage);
            };
        }
    }
}