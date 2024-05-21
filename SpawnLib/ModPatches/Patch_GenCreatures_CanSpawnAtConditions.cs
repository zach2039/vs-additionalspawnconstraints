using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.Server;
using Vintagestory.ServerMods;

namespace SpawnLib.ModPatches
{
	[HarmonyPatch(typeof(GenCreatures), "CanSpawnAtConditions")]
	public class Patch_GenCreatures_CanSpawnAtConditions
	{
		static bool Prefix(GenCreatures __instance, ref bool __result, IBlockAccessor blockAccessor, EntityProperties type, BlockPos pos, Vec3d posAsVec, BaseSpawnConditions sc, float rain, float temp, float forestDensity, float shrubsDensity, ref ICoreServerAPI ___api)
		{
			return SpawnLibModSystem.CheckSpawnAt(___api, pos, type, ref __result);
		}
    }
}