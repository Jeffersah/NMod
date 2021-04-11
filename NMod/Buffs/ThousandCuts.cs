using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RoR2.CharacterBody;

namespace NMod.Buffs
{
    [NModBuff]
    class ThousandCuts : CustomBuffBase
    {
        public static string Name = "thousandcuts_buff";
        public override string InternalName => Name;
        public override string AssetPath => "thousandcuts/thousandcuts_icon.png";

        public override bool IsDebuff => false;
        public override bool CanStack => true;
    }
}
