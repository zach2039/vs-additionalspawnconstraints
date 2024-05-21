using Vintagestory.API.Common;
using Vintagestory.API.Server;
using AdditionalSpawnConstraints.ModPatches;
using HarmonyLib;
using Vintagestory.Server;
using Vintagestory.ServerMods;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace AdditionalSpawnConstraints
{
	public class AdditionalSpawnConstraintsModSystem : ModSystem
	{
		public Harmony harmonyInst;

		internal void PatchGenCreaturesCanSpawnAtConditions(ICoreServerAPI sapi, Harmony harmony)
		{
			var original = typeof(GenCreatures).GetMethod("CanSpawnAtConditions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var prefix = typeof(Patch_GenCreatures_CanSpawnAtConditions).GetMethod("Prefix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
			harmony.Patch(original, new HarmonyMethod(prefix), null);			

			sapi.Logger.Notification("Applied patch to VintageStory's GenCreatures.CanSpawnAtConditions from AdditionalSpawnConstraints!");		
		}

		internal void PatchServerSystemEntitySpawnerCanSpawnAt(ICoreServerAPI sapi, Harmony harmony)
		{
			var original = typeof(ServerSystemEntitySpawner).GetMethod("CanSpawnAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var prefix = typeof(Patch_ServerSystemEntitySpawner_CanSpawnAt).GetMethod("Prefix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
			harmony.Patch(original, new HarmonyMethod(prefix), null);			

			sapi.Logger.Notification("Applied patch to VintageStory's ServerSystemEntitySpawner.CanSpawnAt from AdditionalSpawnConstraints!");		
		}

		internal void PatchBlockCanCreatureSpawnOn(ICoreServerAPI sapi, Harmony harmony)
		{
			var original = typeof(Block).GetMethod("CanCreatureSpawnOn", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			var postfix = typeof(Patch_Block_CanCreatureSpawnOn).GetMethod("Postfix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
			harmony.Patch(original, null, new HarmonyMethod(postfix));			

			sapi.Logger.Notification("Applied patch to VintageStory's Block.CanCreatureSpawnOn from AdditionalSpawnConstraints!");		
		}

		public override void StartServerSide(ICoreServerAPI sapi)
		{
			if (!Harmony.HasAnyPatches(Mod.Info.ModID)) {
				harmonyInst = new Harmony(Mod.Info.ModID);

				PatchServerSystemEntitySpawnerCanSpawnAt(sapi, harmonyInst);

				PatchBlockCanCreatureSpawnOn(sapi, harmonyInst);

				PatchGenCreaturesCanSpawnAtConditions(sapi, harmonyInst);
			}

			base.StartServerSide(sapi);

			sapi.Logger.Notification("Loaded AdditionalSpawnConstraints!");
		}

		/// <summary>
		/// Meant to be called from within harmony patch for methods that evaluate spawn conditions
		/// </summary>
		/// <param name="sapi"></param>
		/// <param name="blockPos"></param>
		/// <param name="type"></param>
		/// <param name="result">harmony method result to change</param>
		/// <returns>whether or not to skip the original method</returns>
		public static bool CheckSpawnAt(ICoreServerAPI sapi, BlockPos blockPos, EntityProperties type, ref bool result)
		{
			if (type.Attributes == null || !type.Attributes.KeyExists("spawnLib"))
			{
				return true; // continue with original method
			}

			if (!type.Attributes.KeyExists("minSpawnRegionStability") && !type.Attributes.KeyExists("maxSpawnRegionStability"))
			{
				return true; // continue with original method
			}

			SystemTemporalStability tempStabilitySystem = sapi.ModLoader.GetModSystem<SystemTemporalStability>(true);
			if (tempStabilitySystem == null)
			{
				return true; // continue with original method
			}

			BlockPos tmpPos = blockPos.Copy();
				
			double hereStability = tempStabilitySystem.GetTemporalStability(tmpPos);

			// Stability can range from 0.0f to 2.0f, I think
			if (type.Attributes.KeyExists("minSpawnRegionStability"))
			{
				double minStability = type.Attributes["minSpawnRegionStability"].AsDouble(0.0f);
				if (hereStability < minStability)
				{
					result = false;
					return false; // skip original method
				}
			}

			if (type.Attributes.KeyExists("maxSpawnRegionStability"))
			{
				double maxStability = type.Attributes["maxSpawnRegionStability"].AsDouble(0.0f);
				if (hereStability > maxStability)
				{
					result = false;
					return false; // skip original method
				}
			}

			return true; // continue with original method
		}

		public static void CanSpawnOn(IBlockAccessor blockAccessor, BlockPos blockPos, EntityProperties type, ref bool result)
		{
			if (type.Attributes == null || !type.Attributes.KeyExists("spawnLib"))
			{
				return;
			}

			if (!type.Attributes.KeyExists("canSpawnOn") && !type.Attributes.KeyExists("cannotSpawnOn"))
			{
				return;
			}

			if (type.Attributes.KeyExists("cannotSpawnOn"))
			{
				Block block = blockAccessor.GetBlock(blockPos);
				bool foundMatch = false;
				foreach (string blockCodeSearch in type.Attributes.AsArray<string>())
				{
					if (WildcardUtil.Match(blockCodeSearch, block.Code.ToString()))
					{
						foundMatch = true;
						break;
					}
				}

				if (foundMatch)
				{
					result = false; // prevent spawning on any match found in cannotSpawnOn
					return;
				}
			}

			if (type.Attributes.KeyExists("canSpawnOn"))
			{
				Block block = blockAccessor.GetBlock(blockPos);
				bool foundMatch = false;
				foreach (string blockCodeSearch in type.Attributes.AsArray<string>())
				{
					if (WildcardUtil.Match(blockCodeSearch, block.Code.ToString()))
					{
						foundMatch = true;
						break;
					}
				}

				if (!foundMatch)
				{
					result = false; // prevent spawning on no match found in canSpawnOn
					return;
				}
			}
		}
	}
}
