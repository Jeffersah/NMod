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

    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
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
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(NModLoader.LoadedCustomItems[Envy.Name].Index), transform.position, transform.forward * 20f);
            }
            if(Input.GetKeyDown(KeyCode.F3))
            {
                //Get the player body to use a position:	
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                var customItems = NModLoader.LoadedCustomItems.Values.ToArray();
                for(var i = 0; i < customItems.Length; i++)
                {
                    var rotation = Math.PI * 2 * (i / (float)customItems.Length);
                    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(customItems[i].Index), transform.position, new Vector3((float)Math.Cos(rotation), 0.05f, (float)Math.Sin(rotation)) * 30f);

                }
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                //Get the player body to use a position:	
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                //And then drop our defined item in front of the player.
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex((ItemIndex)53), transform.position, transform.forward * 20f);
            }
        }
    }
}
