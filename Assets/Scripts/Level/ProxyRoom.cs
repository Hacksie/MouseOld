using System;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign.Level
{

    [System.Serializable]
    public class ProxyRoom
    {
        public bool isEntry = false;
        public bool isEnd = false;
        public bool isMainChain = false;
        public bool isNearEntry = false;

        public string floor = "";
        public string top = "";
        public string left = "";
        public string bottom = "";
        public string right = "";

        // We'd use an enum, but I'm too lazy to write a serializer
        public const string Wall = "w";
        public const string Open = "o";
        public const string Door = "d";
        public const string Exit = "e";
        public const string Entry = "n";
        public const string Any = "a";
        public const string OpenOrDoor = "x";
        public const string OpenOrWall = "y";
        public const string DoorOrWall = "z";

        public const string ObjTypeWall = "wall";
        public const string ObjTypeEntry = "entry";
        public const string ObjTypeEnd = "end";
        public const string ObjTypeTrap = "trap";
        public const string ObjTypeRandom = "random";
        public const string ObjTypeFixed = "fixed";



        public List<Corner> bottomLeft = new List<Corner>();
        public List<Corner> bottomRight = new List<Corner>();
        public List<Corner> topLeft = new List<Corner>();
        public List<Corner> topRight = new List<Corner>();

        // Set at runtime    
        //public bool visited = false;     

        // FIXME: Create individual as strings
        public override string ToString()
        {
            string s = "" + left + top + bottom + right;
            return s;
        }
    }


    [System.Serializable]
    public class Corner
    {
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