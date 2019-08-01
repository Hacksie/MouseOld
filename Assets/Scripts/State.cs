
namespace HackedDesign {

    public class State {
        public GameState state = GameState.LOADING;

        public State () {
            this.state = GameState.LOADING;
        }

        public State (GameState startingState) {
            this.state = startingState;
        }

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