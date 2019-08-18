using System;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Level {
        [System.Serializable]
        public class ProxyRoom {
            public bool isEntry = false;
            public bool isEnd = false;
            public bool isMainChain = false;
            public bool isNearEntry = false;

            public bool visited = false;

            public string top;
            public string left;
            public string bottom;
            public string right;

            // We'd use an enum, but I'm too lazy to write a serializer
            public const string WALL = "w";
            public const string OPEN = "o";
            public const string DOOR = "d";
            public const string EXIT = "e"; 

            public const string OBJ_TYPE_WALLS = "w";
            public const string OBJ_TYPE_ENTRY = "e";
            public const string OBJ_TYPE_END = "x";
            public const string OBJ_TYPE_TRAP = "t";
            public const string OBJ_TYPE_RANDOM = "r";

                        

            public List<Corner> bottomLeft = new List<Corner> ();
            public List<Corner> bottomRight = new List<Corner> ();
            public List<Corner> topLeft = new List<Corner> ();
            public List<Corner> topRight = new List<Corner> ();

            public string AsPrintableString () {
                string s = "" + left + top + bottom + right;
                return s;
            }

            // public char SideToString (RoomSide side) {
            //     return (char) side;
            // }
        }

        // public enum RoomSide {
        //     Wall = 'w',
        //     Open = 'o',
        //     Door = 'd',
        //     Exit = 'e'
        // }

        [System.Serializable]
        public class Corner {
            public string type;
            public string name;
            public bool isTrap;
        }

        [System.Serializable]
        public class ProxyRow
        {
            public ProxyRoom[] rooms;
        }        
    }
}