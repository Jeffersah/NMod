using BepInEx.Logging;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NMod.Buffs
{
    public abstract class CustomBuffBase
    {
        private const string ModPrefix = ""; //"@NMod:";
        public abstract string InternalName { get; }
        public virtual string AssetPath => null;
        public virtual bool CanStack => true;
        public abstract bool IsDebuff { get; }
        public abstract Color BuffColor { get; }

        public string IconPath => $"{ModPrefix}Assets/Import/{AssetPath ?? (InternalName + "/" + InternalName + "_buff.png")}";

        public BuffIndex Index { get; set; }

        protected static ManualLogSource Log => NModMain.Log;

        public virtual ItemDisplayRule[] GetItemDisplayRules(GameObject prefab)
        {
            return null;
        }
        public virtual void RegisterHooks(BuffIndex itemIndex)
        {

        }
        public virtual void Update()
        {

        }
    }
}
