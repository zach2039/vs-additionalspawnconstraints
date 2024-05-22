using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.Server;

namespace AdditionalSpawnConstraints.ModPatches
{
	[HarmonyPatch(typeof(SystemTemporalStability), "DoSpawn")]
	public class Patch_SystemTemporalStability_DoSpawn
	{
		static bool Prefix(ServerSystemEntitySpawner __instance, EntityProperties entityType, Vec3d spawnPosition, long herdid, ref ICoreAPI ___api)
		{
            if (entityType.Attributes == null || !entityType.Attributes.KeyExists("additionalSpawnConstraints"))
			{
				return true; // resume original method
			}

			if (!entityType.Attributes["additionalSpawnConstraints"].KeyExists("changeTemporalStormSpawnMechanics") || !entityType.Attributes["additionalSpawnConstraints"]["changeTemporalStormSpawnMechanics"].AsBool(false))
            {
                return true; // resume original method
            }

			bool canSpawn = true;
			AdditionalSpawnConstraintsModSystem.CanSpawnOn(___api.World.BlockAccessor, spawnPosition.AsBlockPos, entityType, ref canSpawn);

			if (!canSpawn)
			{
				return false; // skip original method
			}

			return true; // resume original method
		}
    }
}