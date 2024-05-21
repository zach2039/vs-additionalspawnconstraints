using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.Server;

namespace AdditionalSpawnConstraints.ModPatches
{
	[HarmonyPatch(typeof(ServerSystemEntitySpawner), "CanSpawnAt")]
	public class Patch_ServerSystemEntitySpawner_CanSpawnAt
	{
		static bool Prefix(ServerSystemEntitySpawner __instance, ref Vec3d __result, EntityProperties type, Vec3i spawnPosition, RuntimeSpawnConditions sc, IWorldChunk[] chunkCol, ref ServerMain ___server)
		{
			bool canSpawn = true;
			bool resumeMethod = AdditionalSpawnConstraintsModSystem.CheckSpawnAt(___server.Api as ICoreServerAPI, spawnPosition.AsBlockPos, type, ref canSpawn);

			if (!canSpawn)
			{
				__result = null;
			}

			return resumeMethod;
		}
    }
}