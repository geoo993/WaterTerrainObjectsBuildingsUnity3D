namespace TerrainGenerator
{
    public interface INoiseProvider
    {
        float GetValue(float x, float z);
    }
}