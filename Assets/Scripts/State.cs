
namespace HackedDesign {

    public struct State {
        public GameState state; // = GameState.LOADING;
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