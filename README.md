Additional Spawn Constraints
=================

A server-side dependency mod that allows JSON mods and patching to control various spawn conditions; does nothing on its own!

Overview
--------

AdditionalSpawnConstraints will look for an `additionalSpawnConstraints` object inside an entity's `/attributes` object. This can be included on new entity JSON files, or patched in via JSON.

An example for some properties is below, applied via a patch to `drifter.json`:

```json
[
	{
		"side": "Server",
		"file": "game:entities/land/drifter.json",
		"op": "addmerge",
		"path": "/attributes/additionalSpawnConstraints",
		"value": {
			"changeTemporalStormSpawnMechanics": true,
			"canSpawnOn":[
				"game:sand-*",
				"game:gravel-*",
				"game:dirtygravel",
				"game:sludgygravel",
				"game:muddygravel",
				"game:soil-*",
				"game:forestfloor-*",
				"game:peat-*",
				"game:rock-*",
				"game:crackedrock-*",
				"game:regolith-*",
				"game:ore-gem-*",
				"game:ore-graded-*",
				"game:ore-ungraded-*"
			]
		}
	}
]
```

The properties that can be set within `additionalSpawnConstraints` are:

- `"minRegionStability"` : The minimum region stablity required when spawning the entity; does not take into account rifts nor rift proximity.

- `"maxRegionStability"` : The maximum region stablity required when spawning the entity; does not take into account rifts nor rift proximity.

- `"canSpawnOn"` : A list of block codes that are valid for spawning the entity on; supports globs and regex to handle variants.

- `"cannotSpawnOn"` : A list of block codes that are not valid for spawning the entity on; supports globs and regex to handle variants.

- `"changeTemporalStormSpawnMechanics"` : Determines if other properties can affect temporal storm spawning, where applicable; default is `false` if unspecified. Does not apply to `"minRegionStability"` or `"maxRegionStability"`. Only affects entities that are spawned during storms.

Future Plans
--------

 - Properties related to health scaling on spawn.

Known Issues
--------

 - None, atm.

Mods That Use This
--------

 - [Natural Spawns](https://mods.vintagestory.at/naturalspawns) : Limits drifters to spawning on soil and other natural blocks, even during storms.

Extras
--------

 - Thanks to Thalius for testing help.
