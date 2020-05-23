namespace HackedDesign.Level
{
    public interface ILevelGenerator
    {
        Level GenerateLevel(LevelGenTemplate template);
        Level GenerateLevel(LevelGenTemplate template, int difficulty, int enemies, int traps);
        Level GenerateLevel(LevelGenTemplate template, int length, int height, int width, int difficulty, int enemies, int traps);
    }
}