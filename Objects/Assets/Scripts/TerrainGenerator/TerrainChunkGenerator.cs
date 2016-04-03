using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace TerrainGenerator
{
    public class TerrainChunkGenerator : MonoBehaviour
    {
//        public Material TerrainMaterial;
//
//        public Texture2D FlatTexture;
//        public Texture2D SteepTexture;
//
//        private TerrainChunkSettings Settings;
//
//        private NoiseProvider NoiseProvider;
//
//        private ChunkCache Cache;
//
//        private void Awake()
//        {
//            Settings = new TerrainChunkSettings(129, 129, 100, 40, FlatTexture, SteepTexture, TerrainMaterial);
//            NoiseProvider = new NoiseProvider();
//
//            Cache = new ChunkCache();
//	
//        }

		void Start()
		{
			Test ();
		}
		void Test()
		{
			var settings = new TerrainChunkSettings(129, 129, 100, 20);
			var noiseProvider = new NoiseProvider();

			//part1
			//var terrain = new TerrainChunk(this.transform,settings, noiseProvider, 0, 0);
			//terrain.CreateTerrain();

			//part2
			for (var i = 0; i < 4; i ++)
				for (var j = 0; j < 4; j++)
					new TerrainChunk(this.transform,settings, noiseProvider, i, j).CreateTerrain();
			
		}
//
//        private void Update()
//		{
//
//            if (Input.GetKeyDown(KeyCode.A))
//            {
//            }
//
//            Cache.Update();
//        }
//
//        private void GenerateChunk(int x, int z)
//        {
//            if (Cache.ChunkCanBeAdded(x, z))
//            {
//                var chunk = new TerrainChunk(Settings, NoiseProvider, x, z);
//                Cache.AddNewChunk(chunk);
//
//            }
//        }
//
//        private void RemoveChunk(int x, int z)
//        {
//            if (Cache.ChunkCanBeRemoved(x, z))
//                Cache.RemoveChunk(x, z);
//        }
//
//		private List<Vector2i> GetChunkPositionsInRadius(Vector2i chunkPosition, int radius)
//        {
//			var result = new List<Vector2i>();
//
//            for (var zCircle = -radius; zCircle <= radius; zCircle++)
//            {
//                for (var xCircle = -radius; xCircle <= radius; xCircle++)
//                {
//                    if (xCircle * xCircle + zCircle * zCircle < radius * radius)
//                        result.Add(new Vector2(chunkPosition.X + xCircle, chunkPosition.Z + zCircle));
//                }
//            }
//
//            return result;
//        }

//
//        public void UpdateTerrain(Vector3 worldPosition, int radius)
//        {
//            var chunkPosition = GetChunkPosition(worldPosition);
//            var newPositions = GetChunkPositionsInRadius(chunkPosition, radius);
//
//            var loadedChunks = Cache.GetGeneratedChunks();
//            var chunksToRemove = loadedChunks.Except(newPositions).ToList();
//
//            var positionsToGenerate = newPositions.Except(chunksToRemove).ToList();
//            foreach (var position in positionsToGenerate)
//                GenerateChunk(position.X, position.Z);
//
//            foreach (var position in chunksToRemove)
//                RemoveChunk(position.X, position.Z);
//        }
//
//        public Vector2i GetChunkPosition(Vector3 worldPosition)
//        {
//            var x = (int)Mathf.Floor(worldPosition.x / Settings.Length);
//            var z = (int)Mathf.Floor(worldPosition.z / Settings.Length);
//
//            return new Vector2i(x, z);
//        }
//
//        public bool IsTerrainAvailable(Vector3 worldPosition)
//        {
//            var chunkPosition = GetChunkPosition(worldPosition);
//            return Cache.IsChunkGenerated(chunkPosition);
//        }
//
//        public float GetTerrainHeight(Vector3 worldPosition)
//        {
//            var chunkPosition = GetChunkPosition(worldPosition);
//            var chunk = Cache.GetGeneratedChunk(chunkPosition);
//            if (chunkPosition != null)
//                return chunk.GetTerrainHeight(worldPosition);
//
//            return 0;
//        }
    }
}