using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class SF //Static Functions
    {
        public static int SW = 1920; //ScreenWidth
        public static int SH = 1080; //ScreenHeight

        public static bool DidIClick(Vector2 clickPos, Vector2 targetPos, Vector2 targetScale)
        {
            bool retVal = true;
            if(clickPos.X > targetPos.X + targetScale.X/2) retVal = false;
            if(clickPos.X < targetPos.X - targetScale.X/2) retVal = false;
            if(clickPos.Y > targetPos.Y + targetScale.Y/2) retVal = false;
            if(clickPos.Y < targetPos.Y - targetScale.Y/2) retVal = false;

            return retVal;
        }
    }
}
