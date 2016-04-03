namespace TerrainGenerator
{
    public class TerrainChunkNeighborhood
    {
        public TerrainChunk XUp { get; set; }

        public TerrainChunk XDown { get; set; }

        public TerrainChunk ZUp { get; set; }

        public TerrainChunk ZDown { get; set; }
    }

    public enum TerrainNeighbor
    {
        XUp,
        XDown,
        ZUp,
        ZDown
    }
}