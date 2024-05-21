Spawn Lib
=================

A server-side dependency mod that allows JSON mods and patching to control various spawn conditions; does nothing on its own!

Overview
--------

SpawnLib will look for a `spawnLib` object inside an entity's `/attributes` object. This can be included on new entity JSON files, or patched in via JSON.

An example for some properties is below, applied via a patch to `drifter.json`:

```json
[
	{
		"side": "Server",
		"file": "game:entities/land/drifer.json",
		"op": "addmerge",
		"path": "/attributes/spawnLib",
		"value": {
			"minRegionStability":0.0,
			"maxRegionStability":1.5,
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

The properties that can be set within `spawnLib` are:

- `"minRegionStability"` : The minimum region stablity required when spawning the entity; does not take into account rifts nor rift proximity.

- `"maxRegionStability"` : The maximum region stablity required when spawning the entity; does not take into account rifts nor rift proximity.

- `"canSpawnOn"` : A list of block codes that are valid for spawning the entity on; supports globs and regex to handle variants.

- `"cannotSpawnOn"` : A list of block codes that are not valid for spawning the entity on; supports globs and regex to handle variants.

Future Plans
--------

 - None, atm.

Known Issues
--------

 - None, atm.
