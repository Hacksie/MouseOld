using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		[CreateAssetMenu (fileName = "LevelElements", menuName = "Mouse/Level/LevelElements")]
		public class LevelElements : ScriptableObject {

			public List<Chunk> chunks;
			
			public ColorPalette colorPalette;

			public Difficulty difficulty;

			public enum ColorPalette {
				Magenta,
				Blue,
				Cyan,
				Beige
			}	

			public enum Difficulty {
				Tutorial,
				Easy,
				Medium,
				Hard
			
			}		

		}
	}
}