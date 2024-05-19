using Vintagestory.API.Common;
using Vintagestory.API.Server;
using StabilitySpawnLib.ModPatches;
using HarmonyLib;
using Vintagestory.Server;

namespace StabilitySpawnLib
{
	public class StabilitySpawnLibModSystem : ModSystem
	{
		public Harmony harmony;

		public override void StartServerSide(ICoreServerAPI sapi)
		{
			if (!Harmony.HasAnyPatches(Mod.Info.ModID)) {
				harmony = new Harmony(Mod.Info.ModID);

                var original = typeof(ServerSystemEntitySpawner).GetMethod("CanSpawnAt", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var prefix = typeof(Patch_ServerSystemEntitySpawner_CanSpawnAt).GetMethod("Prefix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                harmony.Patch(original, new HarmonyMethod(prefix), null);			

                sapi.Logger.Notification("Applied patch to VintageStory's ServerSystemEntitySpawner.CanSpawnAt from StabilitySpawnLib!");				
			}

			base.StartServerSide(sapi);

			sapi.Logger.Notification("Loaded StabilitySpawnLib!");
		}
	}
}
