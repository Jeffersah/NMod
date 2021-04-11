using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using static RoR2.CharacterBody;

namespace NMod.Items
{
    [NModItem(
          "Thousand Cuts",
          "Base damage increases over time while in combat. Resets if you take damage or exit combat.",
          "Gain <style=cIsUtility>1%</style> (+.5% per stack) damage per second while in combat, to a maximum of <style=cIsUtility>5%</style> (+2.5% per stack). Resets when you leave combat or take damage.")]
    class ThousandCuts : CustomBehaviorItemBase<ThousandCuts.Behavior>
    {
        const float DAMAGE_BOOST_PER_BUFF_STACK = 0.005f;
        const int TICKS_PER_STACK = 60;

        const int BASE_DMG_BONUS = 2;
        const int STACK_DMG_BONUS = 1;

        const int MAX_BASE_DMG_BONUS = 10;
        const int MAX_STACK_DMG_BONUS = 5;

        const int OUT_OF_COMBAT_TICKS = 60 * 2;
        private static BuffIndex buffIndex => NModLoader.LoadedCustomBuffs[Buffs.ThousandCuts.Name].Index;
        public static string Name => nameof(ThousandCuts).ToLower();
        public override string InternalName => Name;
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] Tags => new ItemTag[] {
            ItemTag.Damage,
        };

        public override void RegisterHooks(ItemIndex itemIndex)
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                if(EventUtils.AttackerIsCharacter(damageInfo, out var attackerBody))
                {
                    EventUtils.TryGetAttackerItemCount(damageInfo, itemIndex, out int attackerItemCount);
                    EventUtils.TryGetDefenderItemCount(self, itemIndex, out int defenderCount);
                    if(attackerItemCount > 0)
                    {
                        attackerBody.GetComponent<Behavior>().ResetCombatTimer();
                        damageInfo.damage *= 1 + DAMAGE_BOOST_PER_BUFF_STACK * attackerBody.GetBuffCount(buffIndex);
                    }
                    if(defenderCount > 0)
                    {
                        self.body.GetComponent<Behavior>().ResetBuffStack();
                    }
                }
                orig(self, damageInfo);
            };
            base.RegisterHooks(itemIndex);
        }

        public class Behavior : ItemBehavior
        {
            int timeUntilCombatTimeout;
            int timeInCombat;
            public Behavior()
            {
                timeUntilCombatTimeout = timeInCombat = 0;
            }
            public void ResetCombatTimer()
            {
                timeUntilCombatTimeout = OUT_OF_COMBAT_TICKS;
            }
            public void ResetBuffStack()
            {
                timeInCombat = 0;
                timeUntilCombatTimeout = 0;
            }

            public void Update()
            {
                var currentBuffCount = body.GetBuffCount(buffIndex);
                if (timeUntilCombatTimeout > 0)
                {
                    timeUntilCombatTimeout--;
                    timeInCombat++;
                    if(timeInCombat >= TICKS_PER_STACK)
                    {
                        timeInCombat = 0;
                        int maxStackQty = StackUtils.LinearStack(stack, MAX_BASE_DMG_BONUS, MAX_STACK_DMG_BONUS);
                        int addStackQty = Math.Min(StackUtils.LinearStack(stack, BASE_DMG_BONUS, STACK_DMG_BONUS), maxStackQty - currentBuffCount);
                        for(; addStackQty > 0; addStackQty--)
                        {
                            body.AddBuff(buffIndex);
                        }
                    }
                }
                else
                {
                    timeInCombat = 0;
                    timeUntilCombatTimeout = 0;
                    while (currentBuffCount-- > 0)
                        body.RemoveBuff(buffIndex);
                }
            }
        }
    }
}
