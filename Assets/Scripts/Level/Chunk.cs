using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class Chunk : ScriptableObject {

			public bool[] open = new bool[4];
			public bool[] door = new bool[4];

			public GameObject prefab;

			public ColorPalette colorPalette;


			public enum ColorPalette {
				Magenta,
				Blue,
				Cyan,
				Beige
			}
		}
	}
}