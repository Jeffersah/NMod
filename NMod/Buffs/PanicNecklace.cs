using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NMod.Buffs
{
    [NModBuff]
    class PanicNecklace : CustomBuffBase
    {
        const float SPEED_PER_STACK = 0.25f;

        private static Dictionary<CharacterBody, float> MoveSpeedAdjusts { get; } = new Dictionary<CharacterBody, float>();

        public static string Name = "panicnecklace_buff";
        public override string InternalName => Name;
        public override string AssetPath => "panicnecklace/panicnecklace_icon.png";

        public override bool IsDebuff => false;
        public override bool CanStack => false;
        public override Color BuffColor => new Color(1, 0.4f, 0.4f);

        public static void SetCharacterMoveSpeedAdjust(CharacterBody forCharacter, float amt)
        {
            MoveSpeedAdjusts.Add(forCharacter, amt);
        }

        public override void RegisterHooks(BuffIndex buffIndex)
        {
            On.RoR2.CharacterBody.OnBuffFirstStackGained += (orig, self, buff) =>
            {
                if(buff.buffIndex == buffIndex && MoveSpeedAdjusts.TryGetValue(self, out float adj))
                {
                    self.baseMoveSpeed += adj;
                }
                orig(self, buff);
            };

            On.RoR2.CharacterBody.OnBuffFinalStackLost += (orig, self, buff) =>
            {
                if (buff.buffIndex == buffIndex && MoveSpeedAdjusts.TryGetValue(self, out float adj))
                {
                    self.baseMoveSpeed -= adj;
                    MoveSpeedAdjusts.Remove(self);
                }
                orig(self, buff);
            };
        }
    }
}