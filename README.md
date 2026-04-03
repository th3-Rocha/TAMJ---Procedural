# TAMJ — Procedural (Unity / C# Study Project)

**TAMJ — Procedural** is a Unity project made **to study C#** by reading and editing scripts.  
The focus is learning common game-dev patterns like procedural generation, coroutines, async-style workflows, mesh creation, and basic world streaming.

## What this project is
- An **infinite/procedural terrain generation simulator** built in **Unity (C#)**.
- Terrain is generated in **chunks** around the player using **multi-octave Perlin noise**.
- There is also a **procedural cave generator** using a **cellular automata** style smoothing process.

## Main mechanics / systems in the code

### 1) Infinite terrain / chunk streaming around the player
The world is generated as the player moves:
- The player position is converted into a **grid position**.
- A set of active chunk positions is kept in a dictionary.
- When the player enters a new grid cell, the system:
  - **Spawns new chunks** that come into view distance.
  - **Destroys old chunks** that fall outside view distance.

**Where to look:**
- `Assets/Scripts/MapGenerator.cs`  
  - `ActiveTerrainsAroundPlayer()` handles spawning/removing chunks.
  - `GenOneChunck()` instantiates a new chunk (TerrainPrefab).
  - `GenerateAllActiveMaps()` generates the initial set.

### 2) Noise-based heightmap generation (multi-octave Perlin noise)
Each chunk gets a heightmap from a noise function:
- Seed + scale + octaves + persistence + lacunarity
- Uses Perlin noise layered in octaves to get more interesting terrain.

**Where to look:**
- `Assets/Scripts/Noise.cs` (`GenerateNoiseMap(...)`)
- `Assets/Scripts/myNoiseMap.cs` (`reGenNoise(...)` calls MapGenerator to regenerate noise for a chunk)

### 3) Mesh generation (turning heightmap into a 3D terrain mesh)
The heightmap is converted into a mesh by generating:
- Vertices
- Triangles
- Normals + UVs

**Where to look:**
- `Assets/Scripts/MeshDrawer.cs` (`DrawMeshAndAttachMeshFilter(...)` and terrain mesh building)

### 4) Async-style chunk generation flow (coroutines)
Chunk generation is staged over frames using coroutines so it doesn’t freeze everything at once:
- Assign/update grid position
- Regenerate noise
- Apply texture
- Generate mesh (sync or async mode)
- Create collider
- Spawn objects

**Where to look:**
- `Assets/Scripts/CombineAndCreate.cs`
  - `Gen()` → starts coroutine
  - Chooses between:
    - default plane workflow (`isDefaultPlane`)
    - mesh workflow (`isAsyncGen` for async mesh path, otherwise sync)

### 5) Texturing terrain by “regions” (biome-like coloring)
The terrain is colored based on height thresholds:
- `TerrainType[] regions` defines height cutoffs and colors
- The chunk gets a generated texture (color map or height map)

**Where to look:**
- `Assets/Scripts/TextureDrawer.cs` (`TexturizeMap(...)`)
- `Assets/Scripts/TextureGenerator.cs` (creates textures)

### 6) Object spawning based on terrain height + probability
Objects (prefabs) can spawn depending on:
- the height region the point belongs to
- a per-region `spawnProbability`
- a spacing/density setting (`ObjRate`)

**Where to look:**
- `Assets/Scripts/ObjSpawner.cs`
  - `StartObjGen()` / `ObjGen()` coroutine
  - `SelectPrefabForExactHeight(...)`

### 7) Procedural cave generation (cellular automata smoothing)
There’s a separate cave system that:
- Randomly fills a grid (walls/empty)
- Smooths it multiple passes (cellular automata rule)
- Produces a cave-like map that can be drawn as mesh

**Where to look:**
- `Assets/Scripts/CelulaGen.cs`
  - `StartCaveGeneration(...)`
  - `RandomFillMap(...)`, `SmoothMap()`, `GetSurroundingWallCount(...)`
- `Assets/genCave.cs`
  - Runs cave generation coroutine
  - Sends the result to a mesh drawer (async path)

### 8) Generate caves only when the player is near (distance trigger)
A cave chunk can be generated only when the player gets close:
- Checks distance to player
- Generates once when inside `triggerDistance`

**Where to look:**
- `Assets/PlayerDist.cs`

### 9) Simple distance-based “LOD disabling”
There is a script that disables objects tagged for LOD when far away.

**Where to look:**
- `Assets/DistantLod.cs`

## Recommended script reading order (for studying C#)
1. `Assets/Scripts/MapGenerator.cs` (core: chunk streaming)
2. `Assets/Scripts/Noise.cs` + `Assets/Scripts/myNoiseMap.cs` (noise pipeline)
3. `Assets/Scripts/CombineAndCreate.cs` (generation orchestration with coroutines)
4. `Assets/Scripts/MeshDrawer.cs` (mesh building fundamentals)
5. `Assets/Scripts/TextureDrawer.cs` + `Assets/Scripts/TextureGenerator.cs` (texture generation)
6. `Assets/Scripts/ObjSpawner.cs` (procedural placement + probability)
7. `Assets/Scripts/CelulaGen.cs` + `Assets/genCave.cs` + `Assets/PlayerDist.cs` (cave system + trigger)

## Notes
- This repository contains many Unity/third-party example assets (like TextMesh Pro examples). The important study code is mostly inside `Assets/Scripts/` plus a few scripts in `Assets/` root.
- Code search results can be incomplete in-chat; if you want to inspect everything, browse the repo’s code on GitHub:
  - `https://github.com/th3-Rocha/TAMJ---Procedural/search?q=extension%3Acs&type=code`
