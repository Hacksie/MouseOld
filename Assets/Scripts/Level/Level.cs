namespace HackedDesign {
	namespace Level {
		public class Level {
            public LevelGenTemplate template;
            public PlaceholderChunk[, ] placeholderLevel;
            public int length;

            public Level(LevelGenTemplate template)
            {
                this.template = template;
                placeholderLevel = new PlaceholderChunk[template.levelWidth, template.levelHeight];

            }
        }
    }
}