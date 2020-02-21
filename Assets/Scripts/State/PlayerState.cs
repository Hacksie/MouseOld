using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.State
{
	[System.Serializable]
    public class PlayerState
    {
        public int movementAugments = 0;
        public int charisma = 0;
        public int intimidation = 0;
        public int software = 0;
        public int hardware = 0;
        public int battery = 50;
        public int maxBattery = 50;
        public int overload = 10;
        public int hack = 5;
        public int maxKeycards = 5;
        public int keycards = 0;
        public int credits = 0;
        public int bugs = 1;
        public int baselevelTimer = 90;

        public bool CanOverload()
        {
            return battery - overload >= 0;
        }

        public bool CanKeycard()
        {
            return keycards > 0;
        }

        public bool CanHack()
        {
            return battery - hack >= 0;
        }

        public bool CanBug()
        {
            return bugs > 0;
        }

        public bool ConsumeOverload()
        {
            if(CanOverload())
            {
                battery -=overload;
                return true;
            }
            return false;
        }

        public bool ConsumeKeycard()
        {
            if(CanKeycard())
            {
                keycards--;
                return true;
            }
            return false;
        }

        public bool ConsumeHack()
        {
            if(CanHack())
            {
                battery -=hack;
                return true;
            }
            return false;
        }
        public bool ConsumeBug()
        {
            if(CanBug())
            {
                --bugs;
                return true;
            }
            return false;
        }        
    }
}