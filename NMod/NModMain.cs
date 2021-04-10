using System;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using NMod.Items;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace NMod
{
    [BepInDependency("com.bepis.r2api")]
    [R2APISubmoduleDependency(nameof(LanguageAPI), nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI), nameof(BuffAPI))]
    //Change these
    [BepInPlugin("com.jeffersah.NMod", "A test mod!", "0.1.15")]
    public class NModMain : BaseUnityPlugin
    {
        private static NModMain _modInstance;
        public static ManualLogSource Log => _modInstance.Logger;

        public void Awake()
        {
            _modInstance = this;
            NModLoader.Init();
        }

        public void Update()
        {
            NModLoader.Update();
            if (Input.GetKeyDown(KeyCode.F2))
            {
                //Get the player body to use a position:	
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                //And then drop our defined item in front of the player.
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(NModLoader.LoadedCustomItems[SnakeEyes.Name].Index), transform.position, transform.forward * 20f);
            }
            if(Input.GetKeyDown(KeyCode.F3))
            {
                //Get the player body to use a position:	
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                foreach(var itemindex in ItemCatalog.allItems)
                {
                    if (ItemCatalog.GetItemDef(itemindex).tier == ItemTier.NoTier) continue;
                    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(itemindex), transform.position, transform.forward * 20f);
                }
            }
        }
    }
}
