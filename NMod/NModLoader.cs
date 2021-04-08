using NMod.Buffs;
using NMod.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NMod
{
    static class NModLoader
    {
        public static Dictionary<string, CustomItemBase> LoadedCustomItems { get; private set; }
        public static Dictionary<string, CustomBuffBase> LoadedCustomBuffs { get; private set; }

        public static void Init()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NMod.assets"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);

                InitBuffs(bundle);
                InitItems(bundle);
            }
        }

        public static void InitBuffs(AssetBundle bundle)
        {
            LoadedCustomBuffs = new Dictionary<string, CustomBuffBase>();
            var customItemTypes = typeof(NModLoader).Assembly.GetTypes().Where(t => t.GetCustomAttribute<NModBuffAttribute>() != null);
            var customBuffs = customItemTypes.Select(t =>
            {
                try
                {
                    return (CustomBuffBase)Activator.CreateInstance(t);
                }
                catch (Exception e)
                {
                    NModMain.Log.LogError("ERROR Initializing " + t.FullName + ":");
                    NModMain.Log.LogError(e.ToString());
                    return null;
                }
            });

            foreach (var customBuff in customBuffs)
            {
                if (customBuff == null) continue;
                LoadAssetsAndInit(customBuff, bundle);
            }

            NModMain.Log.LogMessage($"CustomItemManager added {customBuffs.Count()} buffs");

            On.RoR2.BuffCatalog.Init += (orig) =>
            {
                orig();

                NModMain.Log.LogWarning($"Buff catalog init");

                foreach (var customBuff in customBuffs)
                {
                    if (customBuff != null)
                    {
                        customBuff.Index = BuffCatalog.FindBuffIndex(customBuff.InternalName);
                        if(customBuff.Index == BuffIndex.None)
                        {
                            NModMain.Log.LogError($"Got NONE Buff Index for " + customBuff.InternalName);
                        }
                        customBuff.RegisterHooks(customBuff.Index);
                        LoadedCustomBuffs.Add(customBuff.InternalName, customBuff);
                    }
                }
            };
        }

        public static void InitItems(AssetBundle bundle)
        {
            LoadedCustomItems = new Dictionary<string, CustomItemBase>();
            var customItemTypes = typeof(NModLoader).Assembly.GetTypes().Where(t => t.GetCustomAttribute<NModItemAttribute>() != null);
            var customItems = customItemTypes.Select(t =>
            {
                try
                {
                    return (CustomItemBase)Activator.CreateInstance(t);
                }
                catch(Exception e)
                {
                    NModMain.Log.LogError("ERROR Initializing " + t.FullName + ":");
                    NModMain.Log.LogError(e.ToString());
                    return null;
                }
            });

            foreach (var customItem in customItems)
            {
                if (customItem == null) continue;
                LoadAssetsAndInit(customItem, bundle);
            }

            NModMain.Log.LogMessage($"CustomItemManager added {customItems.Count()} items");

            On.RoR2.ItemCatalog.Init += (orig) =>
            {
                orig();

                NModMain.Log.LogWarning($"Item catalog init");

                // ItemCatalog has itemdefs now

                foreach (var customItem in customItems)
                {
                    if(customItem != null)
                    {
                        customItem.Index = ItemCatalog.FindItemIndex(customItem.InternalName);
                        customItem.RegisterHooks(customItem.Index);
                        LoadedCustomItems.Add(customItem.InternalName, customItem);
                    }
                }
            };
        }

        private static void LoadAssetsAndInit(CustomItemBase item, AssetBundle bundle)
        {
            GameObject prefab = bundle.LoadAsset<GameObject>(item.PrefabPath);
            Sprite pickupSprite = bundle.LoadAsset<Sprite>(item.IconPath);

            if (prefab == null)
            {
                NModMain.Log.LogError($"NModLoader Found no Prefab at {item.PrefabPath}");
            }

            if (pickupSprite == null)
            {
                NModMain.Log.LogError($"NModLoader Found no Sprite at {item.IconPath}");
            }

            var itemDef = ScriptableObject.CreateInstance<ItemDef>();

            itemDef.name = item.InternalName;
            itemDef.pickupModelPrefab = prefab;
            itemDef.pickupIconSprite = pickupSprite;
            itemDef.nameToken = item.NameToken;
            itemDef.pickupToken = item.PickupToken;
            itemDef.descriptionToken = item.DescriptionToken;
            itemDef.loreToken = item.LoreToken;
            itemDef.canRemove = item.CanRemove;
            itemDef.hidden = item.Hidden;
            itemDef.tier = item.Tier;

            var displayRules = item.GetItemDisplayRules(prefab);
            var ci = new CustomItem(itemDef, displayRules);
            if (!ItemAPI.Add(ci))
            {
                NModMain.Log.LogError($"ItemAPI.Add returned false for {item.InternalName}");
            }

            var attrib = item.GetType().GetCustomAttribute<NModItemAttribute>();
            LanguageAPI.Add(item.NameToken, attrib.FriendlyName);
            LanguageAPI.Add(item.PickupToken, attrib.PickupText);
            LanguageAPI.Add(item.DescriptionToken, attrib.Description);
            LanguageAPI.Add(item.LoreToken, attrib.LoreText);
        }

        private static void LoadAssetsAndInit(CustomBuffBase buff, AssetBundle bundle)
        {
            Sprite icon = bundle.LoadAsset<Sprite>(buff.IconPath);

            if (icon == null)
            {
                NModMain.Log.LogError($"NModLoader Found no Sprite at {buff.IconPath}");
            }

            var buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.name = buff.InternalName;
            buffDef.iconSprite = icon;
            buffDef.canStack = buff.CanStack;
            buffDef.isDebuff = buff.IsDebuff;
            buffDef.buffColor = buff.BuffColor;
            buffDef.eliteDef = null;
            // TODO: Elite Buffs

            if (!BuffAPI.Add(new CustomBuff(buffDef)))
            {
                NModMain.Log.LogError($"BuffAPI.Add returned false for {buff.InternalName}");
            }
        }

        public static void Update()
        {
            foreach (var value in LoadedCustomItems.Values) value.Update();
            foreach (var value in LoadedCustomBuffs.Values) value.Update();
        }
    }
}
