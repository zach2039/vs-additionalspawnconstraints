using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.Server;

namespace StabilitySpawnLib.ModPatches
{
	[HarmonyPatchCategory("StabilitySpawnLib_ServerSystemEntitySpawner")]
	[HarmonyPatch(typeof(ServerSystemEntitySpawner), "CanSpawnAt")]
	public class Patch_ServerSystemEntitySpawner_CanSpawnAt
	{
		static bool Prefix(ServerSystemEntitySpawner __instance, ref Vec3d __result, EntityProperties type, Vec3i spawnPosition, RuntimeSpawnConditions sc, IWorldChunk[] chunkCol, ref ServerMain ___server)
		{
			// CanSpawnAt should return null on not spawnable
			SystemTemporalStability tempStabilitySystem = ___server.Api.ModLoader.GetModSystem<SystemTemporalStability>(true);
			if (type.Attributes != null && tempStabilitySystem != null)
			{
				BlockPos tmpPos = new BlockPos(spawnPosition);
				
				double hereStability = tempStabilitySystem.GetTemporalStability(tmpPos);

				// Stability ranges from 0f to 1.5f
				if (type.Attributes.KeyExists("minSpawnRegionStability"))
				{
					double minStability = type.Attributes["minSpawnRegionStability"].AsDouble(0.0f);
					if (hereStability < minStability)
					{
						__result = null;
						return false; // skip original method
					}
				}
				if (type.Attributes.KeyExists("maxSpawnRegionStability"))
				{
					double maxStability = type.Attributes["maxSpawnRegionStability"].AsDouble(0.0f);
					if (hereStability > maxStability)
					{
						__result = null;
						return false; // skip original method
					}
				}
			}
			return true; // continue with original method
		}
    }
}