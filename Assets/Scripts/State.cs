
namespace HackedDesign {

    public class State {
		public int gameSlot;
        public GameState state; 
		public Level.Level level;
    }

	public enum GameState {
		MAINMENU,
		CUTSCENE,
		PLAYING,
		LOADING,
		NARRATION,
		DIALOGUE,
		WORLDMAP,
		STARTMENU,
		SELECTMENU,
		GAMEOVER

	}    
}