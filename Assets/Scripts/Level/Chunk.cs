using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Level {

		[CreateAssetMenu (fileName = "Chunk", menuName = "Mouse/Level/Chunk")]
		public class Chunk: ScriptableObject {

			public bool isEntry;
			public bool isEnd;

			public ChunkSide top;
			public ChunkSide left;
			public ChunkSide bottom;
			public ChunkSide right;

			public GameObject gameObject;

			public ColorPalette colorPalette;

			public enum ChunkSide {
				Wall,
				Open,
				Door
			}


			public enum ColorPalette {
				Magenta,
				Blue,
				Cyan,
				Beige
			}
		}
	}
}