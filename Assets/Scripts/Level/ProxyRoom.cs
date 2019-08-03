namespace HackedDesign {
    namespace Level {
        public class ProxyRoom {
            public bool isEntry = false;
            public bool isEnd = false;
            public bool isMainChain = false;
            public bool isNearEntry = false;

            public bool visited = false;

            public RoomSide top;
            public RoomSide left;
            public RoomSide bottom;
            public RoomSide right;

            public string AsPrintableString () {
                string s = "" + SideToString(left) + SideToString(top) + SideToString(bottom) + SideToString(right);
                return s;
            }

            public char SideToString(RoomSide side)
            {
                return (char)side;
            }
        }

        public enum RoomSide {
            Wall = 'w',
            Open = 'o',
            Door = 'd'
        }   

    }
}