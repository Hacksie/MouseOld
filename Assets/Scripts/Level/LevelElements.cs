using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		[CreateAssetMenu (fileName = "LevelElements", menuName = "Mouse/Level/LevelElements")]
		public class LevelElements : ScriptableObject {

			List<Chunk> chunks;
			
			public ColorPalette colorPalette;

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