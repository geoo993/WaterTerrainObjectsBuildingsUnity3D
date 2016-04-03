using System.Threading;
using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainChunk
    {
        public int X { get; private set; }

		public int Z { get; private set; }

        private Terrain Terrain { get; set; }

		private Transform Parr { get; set; }

       // private TerrainData Data { get; set; }

        private TerrainChunkSettings Settings { get; set; }

        private NoiseProvider NoiseProvider { get; set; }

//        private TerrainChunkNeighborhood Neighborhood { get; set; }
//
//        private float[,] Heightmap { get; set; }
//
//        private object HeightmapThreadLockObject { get; set; }
//

		public TerrainChunk(Transform parent, TerrainChunkSettings settings, NoiseProvider noiseProvider, int x, int z)
        {
            //HeightmapThreadLockObject = new object();

            Settings = settings;
            NoiseProvider = noiseProvider;
            //Neighborhood = new TerrainChunkNeighborhood();

			X = x;
			Z = z;
			Parr = parent ;
        }
//
//        #region Heightmap stuff
//
//        public void GenerateHeightmap()
//        {
//            var thread = new Thread(GenerateHeightmapThread);
//            thread.Start();
//        }
//
//        private void GenerateHeightmapThread()
//        {
//            lock (HeightmapThreadLockObject)
//            {
//                var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];
//
//                for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
//                {
//                    for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
//                    {
//                        var xCoordinate = Position.X + (float)xRes / (Settings.HeightmapResolution - 1);
//                        var zCoordinate = Position.Z + (float)zRes / (Settings.HeightmapResolution - 1);
//
//                        heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
//                    }
//                }
//
//                Heightmap = heightmap;
//            }
//        }
//
//        public bool IsHeightmapReady()
//        {
//            return Terrain == null && Heightmap != null;
//        }
//
//        public float GetTerrainHeight(Vector3 worldPosition)
//        {
//            return Terrain.SampleHeight(worldPosition);
//        }
//
//        #endregion
//
//        #region Main terrain generation
//
//        public void CreateTerrain()
//        {
//            Data = new TerrainData();
//            Data.heightmapResolution = Settings.HeightmapResolution;
//            Data.alphamapResolution = Settings.AlphamapResolution;
//            Data.SetHeights(0, 0, Heightmap);
//            ApplyTextures(Data);
//
//            Data.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);
//            var newTerrainGameObject = Terrain.CreateTerrainGameObject(Data);
//            newTerrainGameObject.transform.position = new Vector3(Position.X * Settings.Length, 0, Position.Z * Settings.Length);
//
//            Terrain = newTerrainGameObject.GetComponent<Terrain>();
//            Terrain.heightmapPixelError = 8;
//            Terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
//            Terrain.materialTemplate = Settings.TerrainMaterial;
//            Terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
//            Terrain.Flush();
//
//        }
		public void CreateTerrain()
		{
			var terrainData = new TerrainData();
			terrainData.heightmapResolution = Settings.HeightmapResolution;
			terrainData.alphamapResolution = Settings.AlphamapResolution;

			var heightmap = GetHeightmap();
			terrainData.SetHeights(0, 0, heightmap);
			terrainData.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);

			var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);
			newTerrainGameObject.transform.position = new Vector3(X * Settings.Length, 0, Z * Settings.Length);
			Terrain = newTerrainGameObject.GetComponent<Terrain>();
			Terrain.Flush();

			Terrain.transform.parent = Parr.transform;
		}

		private float[,] GetHeightmap()
		{
			var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

			for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
			{
				for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
				{
					var xCoordinate = X + (float)xRes / (Settings.HeightmapResolution - 1);
					var zCoordinate = Z + (float)zRes / (Settings.HeightmapResolution - 1);

					//add perlinNoise
					heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
				}
			}

			return heightmap;
		}

//
//        private void ApplyTextures(TerrainData terrainData)
//        {
//            var flatSplat = new SplatPrototype();
//            var steepSplat = new SplatPrototype();
//
//            flatSplat.texture = Settings.FlatTexture;
//            steepSplat.texture = Settings.SteepTexture;
//
//            terrainData.splatPrototypes = new SplatPrototype[]
//            {
//                flatSplat,
//                steepSplat
//            };
//
//            terrainData.RefreshPrototypes();
//
//            var splatMap = new float[terrainData.alphamapResolution, terrainData.alphamapResolution, 2];
//
//            for (var zRes = 0; zRes < terrainData.alphamapHeight; zRes++)
//            {
//                for (var xRes = 0; xRes < terrainData.alphamapWidth; xRes++)
//                {
//                    var normalizedX = (float)xRes / (terrainData.alphamapWidth - 1);
//                    var normalizedZ = (float)zRes / (terrainData.alphamapHeight - 1);
//
//                    var steepness = terrainData.GetSteepness(normalizedX, normalizedZ);
//                    var steepnessNormalized = Mathf.Clamp(steepness / 1.5f, 0, 1f);
//
//                    splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
//                    splatMap[zRes, xRes, 1] = steepnessNormalized;
//                }
//            }
//
//            terrainData.SetAlphamaps(0, 0, splatMap);
//        }
//        #endregion
//
//        #region Distinction
//
//        public override int GetHashCode()
//        {
//            return Position.GetHashCode();
//        }
//
//        public override bool Equals(object obj)
//        {
//            var other = obj as TerrainChunk;
//            if (other == null)
//                return false;
//
//            return this.Position.Equals(other.Position);
//        }
//
//        #endregion
//
//        #region Chunk removal
//
//        public void Remove()
//        {
//            Heightmap = null;
//            Settings = null;
//
//            if (Neighborhood.XDown != null)
//            {
//                Neighborhood.XDown.RemoveFromNeighborhood(this);
//                Neighborhood.XDown = null;
//            }
//            if (Neighborhood.XUp != null)
//            {
//                Neighborhood.XUp.RemoveFromNeighborhood(this);
//                Neighborhood.XUp = null;
//            }
//            if (Neighborhood.ZDown != null)
//            {
//                Neighborhood.ZDown.RemoveFromNeighborhood(this);
//                Neighborhood.ZDown = null;
//            }
//            if (Neighborhood.ZUp != null)
//            {
//                Neighborhood.ZUp.RemoveFromNeighborhood(this);
//                Neighborhood.ZUp = null;
//            }
//
//            if (Terrain != null)
//                GameObject.Destroy(Terrain.gameObject);
//        }
//
//        public void RemoveFromNeighborhood(TerrainChunk chunk)
//        {
//            if (Neighborhood.XDown == chunk)
//                Neighborhood.XDown = null;
//            if (Neighborhood.XUp == chunk)
//                Neighborhood.XUp = null;
//            if (Neighborhood.ZDown == chunk)
//                Neighborhood.ZDown = null;
//            if (Neighborhood.ZUp == chunk)
//                Neighborhood.ZUp = null;
//        }
//
//        #endregion
//
//        #region Neighborhood
//
//        public void SetNeighbors(TerrainChunk chunk, TerrainNeighbor direction)
//        {
//            if (chunk != null)
//            {
//                switch (direction)
//                {
//                    case TerrainNeighbor.XUp:
//                        Neighborhood.XUp = chunk;
//                        break;
//
//                    case TerrainNeighbor.XDown:
//                        Neighborhood.XDown = chunk;
//                        break;
//
//                    case TerrainNeighbor.ZUp:
//                        Neighborhood.ZUp = chunk;
//                        break;
//
//                    case TerrainNeighbor.ZDown:
//                        Neighborhood.ZDown = chunk;
//                        break;
//                }
//            }
//        }
//
//        public void UpdateNeighbors()
//        {
//            if (Terrain != null)
//            {
//                var xDown = Neighborhood.XDown == null ? null : Neighborhood.XDown.Terrain;
//                var xUp = Neighborhood.XUp == null ? null : Neighborhood.XUp.Terrain;
//                var zDown = Neighborhood.ZDown == null ? null : Neighborhood.ZDown.Terrain;
//                var zUp = Neighborhood.ZUp == null ? null : Neighborhood.ZUp.Terrain;
//                Terrain.SetNeighbors(xDown, zUp, xUp, zDown);
//                Terrain.Flush();
//            }
//        }
//
//        #endregion
    }
}