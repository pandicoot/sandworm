public abstract class Generator : IGenerator
{
    public Biome Biome { get; }

    public Generator(Biome biome)
    {
        Biome = biome;
    }

    public abstract void ApplyGenerator(SpatialArray<Tiles> world, int[] surfaceLevels);
}
