using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {

		public interface ILevelGenerator {
            List<Vector2Int> PossibleMovementDirections (Vector2Int pos, ProxyChunk[, ] placeholderLevel);
            Vector3 ConvertLevelPosToWorld(Vector2Int pos, LevelGenTemplate levelGenTemplate);
            LevelGenTemplate GetLevelGenTemplate();

        }
    }
}