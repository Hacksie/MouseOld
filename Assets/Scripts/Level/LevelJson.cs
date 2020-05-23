using System.Collections.Generic;


namespace HackedDesign.Level
{
    [System.Serializable]
    public struct LevelJson
    {
        public string floor;
        public LevelRow[] map;
    }

    [System.Serializable]
    public struct LevelRow
    {
        public List<ProxyRoom> row;
    }

    [System.Serializable]
    public struct LevelRoom
    {
        public string walls;
    }
}