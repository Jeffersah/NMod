using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod
{
    static class EventUtils
    {
        public static bool TryGetAttackerItemCount(DamageInfo dmgInfo, ItemIndex index, out int count)
        {
            if(dmgInfo.attacker && dmgInfo.attacker.GetComponent<CharacterBody>() is CharacterBody body && body != null && body && body.inventory)
            {
                count = body.inventory.GetItemCount(index);
                return true;
            }
            count = 0;
            return false;
        }
        public static bool TryGetDefenderItemCount(HealthComponent self, ItemIndex index, out int count)
        {
            if (self.body && self.body.inventory)
            {
                count = self.body.inventory.GetItemCount(index);
                return true;
            }
            count = 0;
            return false;
        }
        public static bool AttackerIsCharacter(DamageInfo info)
        {
            return info.attacker && info.attacker.GetComponent<CharacterBody>() is CharacterBody body && body != null && body;
        }
        public static bool AttackerIsCharacter(DamageInfo info, out CharacterBody body)
        {
            if (info.attacker)
            {
                body = info.attacker.GetComponent<CharacterBody>();
                if (body != null && body) return true;
            }
            body = null;
            return false;
        }
    }
}
