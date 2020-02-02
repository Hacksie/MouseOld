﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.Character
{
	[System.Serializable]
    public class PlayerState
    {
        public int movementAugments = 0;
        public int charisma = 0;
        public int intimidation = 0;
        public int software = 0;
        public int hardware = 0;
        public int battery = 20;
        public int maxBattery = 50;
        public int zap = 10;
    }
}