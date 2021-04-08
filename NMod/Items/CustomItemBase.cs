using BepInEx.Logging;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NMod.Items
{
    public abstract class CustomItemBase
    {
        private const string ModPrefix = ""; //"@NMod:";
        public abstract string InternalName { get; }
        public virtual string AssetName => null;

        public string PrefabPath => $"{ModPrefix}Assets/Import/{AssetName ?? InternalName}/{AssetName ?? InternalName}.prefab";
        public string IconPath => $"{ModPrefix}Assets/Import/{AssetName ?? InternalName}/{AssetName ?? InternalName}_icon.png";

        public virtual string NameToken => $"{InternalName}_NAME";
        public virtual string PickupToken => $"{InternalName}_PICKUP";
        public virtual string DescriptionToken => $"{InternalName}_DESC";
        public virtual string LoreToken => $"{InternalName}_LORE";
        public virtual bool CanRemove => false;
        public virtual bool Hidden => false;
        public abstract ItemTier Tier { get; }

        public ItemIndex Index { get; set; }

        protected static ManualLogSource Log => NModMain.Log;

        public virtual ItemDisplayRule[] GetItemDisplayRules(GameObject prefab)
        {
            return null;
        }
        public virtual void RegisterHooks(ItemIndex itemIndex)
        {

        }
        public virtual void Update()
        {

        }
    }
}
