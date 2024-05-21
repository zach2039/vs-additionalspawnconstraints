using Vintagestory.API.Common;
using Vintagestory.API.Server;
using SpawnLib.ModPatches;
using HarmonyLib;
using Vintagestory.Server;

namespace SpawnLib
{
	public class SpawnLibModSystem : ModSystem
	{
		public Harmony harmonyInst;

		internal void PatchCanSpawnAt(ICoreServerAPI sapi, Harmony harmony)
		{
			var original = typeof(ServerSystemEntitySpawner).GetMethod("CanSpawnAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var prefix = typeof(Patch_ServerSystemEntitySpawner_CanSpawnAt).GetMethod("Prefix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
			harmony.Patch(original, new HarmonyMethod(prefix), null);			

			sapi.Logger.Notification("Applied patch to VintageStory's ServerSystemEntitySpawner.CanSpawnAt from SpawnLib!");		
		}

		public override void StartServerSide(ICoreServerAPI sapi)
		{
			if (!Harmony.HasAnyPatches(Mod.Info.ModID)) {
				harmonyInst = new Harmony(Mod.Info.ModID);

				PatchCanSpawnAt(sapi, harmonyInst);
			}

			base.StartServerSide(sapi);

			sapi.Logger.Notification("Loaded SpawnLib!");
		}
	}
}
