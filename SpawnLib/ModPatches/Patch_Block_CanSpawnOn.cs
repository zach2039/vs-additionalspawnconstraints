

using System.Collections.Generic;
using System.Text.Json.Nodes;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace SpawnLib.ModPatches
{
	[HarmonyPatch(typeof(Block), "CanCreatureSpawnOn")]
	public class Patch_Block_CanCreatureSpawnOn
	{
		static void Postfix(Block __instance, ref bool __result, IBlockAccessor blockAccessor, BlockPos pos, EntityProperties type, BaseSpawnConditions sc)
		{
			SpawnLibModSystem.CanSpawnOn(blockAccessor, pos, type, ref __result);
		}
	}
}
    