namespace HackedDesign {
    namespace Level {
        public class PlaceholderChunk {
            public bool isEntry = false;
            public bool isEnd = false;
            public bool isMainChain = false;

            public Chunk.ChunkSide top;
            public Chunk.ChunkSide left;
            public Chunk.ChunkSide bottom;
            public Chunk.ChunkSide right;

            public string AsPrintableString () {
                string s = ""; 

                switch (left) {
                    case Chunk.ChunkSide.Wall:
                        s += "W";
                        break;
                    case Chunk.ChunkSide.Door:
                        s += "D";
                        break;
                    case Chunk.ChunkSide.Open:
                        s += "O";
                        break;
                }

                switch (top) {
                    case Chunk.ChunkSide.Wall:
                        s += "W";
                        break;
                    case Chunk.ChunkSide.Door:
                        s += "D";
                        break;
                    case Chunk.ChunkSide.Open:
                        s += "O";
                        break;
                }

                switch (bottom) {
                    case Chunk.ChunkSide.Wall:
                        s += "W";
                        break;
                    case Chunk.ChunkSide.Door:
                        s += "D";
                        break;
                    case Chunk.ChunkSide.Open:
                        s += "O";
                        break;
                }

                switch (right) {
                    case Chunk.ChunkSide.Wall:
                        s += "W";
                        break;
                    case Chunk.ChunkSide.Door:
                        s += "D";
                        break;
                    case Chunk.ChunkSide.Open:
                        s += "O";
                        break;
                }

                return s;
            }
        }
    }
}