using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using static RoR2.CharacterBody;

namespace NMod.Items
{
    [NModItem(
        "Momentum Mori",
        "Kills temporarily increase your max health",
        "Increases your max HP by <style=cIsUtility>10%</style> (+5% per stack) for <style=cIsUtility>5 seconds</style> after you kill an enemy. Kills refresh the cooldown")]
    class Momentum : CustomBehaviorItemBase<Momentum.Behavior>
    {
        public const float BASE_HEALTH_BONUS = 0.1f;
        public const float STACK_HEALTH_BONUS = 0.05f;

        const float TIMEOUT_TIME_BASE = 5;
        const float TIMEOUT_TIME_STACK = 0;

        public static string Name => "momentum";
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier1;
        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Utility,
            ItemTag.AIBlacklist,
            ItemTag.OnKillEffect
        };

        private static BuffIndex buffIndex => NModLoader.LoadedCustomBuffs[Buffs.Momentum.Name].Index;

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += (On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport) =>
            {
                orig(self, damageReport);
                if (damageReport != null && damageReport.attacker != null && damageReport.attackerBody && damageReport.attackerBody.inventory)
                {
                    damageReport.attackerBody.GetComponent<Behavior>()?.ResetTimer();
                }
            };
            base.RegisterHooks(itemIndex);
        }

        public class Behavior : ItemBehavior
        {
            int timeoutTimer;

            public void ResetTimer()
            {
                timeoutTimer = (int)(StackUtils.LinearStack(stack, TIMEOUT_TIME_BASE, TIMEOUT_TIME_STACK) * 60);
                var addBuffCount = stack - body.GetBuffCount(buffIndex);
                while(addBuffCount-- > 0)
                {
                    body.AddBuff(buffIndex);
                }
            }

            public void Update()
            {
                if(timeoutTimer > 0)
                {
                    if(--timeoutTimer == 0)
                    {
                        var rmCount = body.GetBuffCount(buffIndex);
                        while (rmCount-- > 0)
                        {
                            body.RemoveBuff(buffIndex);
                        }
                    }
                }
            }
        }
    }
}
