using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class Automaty
    {
        static Vector2 SPINpos = new Vector2(SF.SW-200, SF.SH/2);
        static Vector2 SPINsize = new Vector2(300, 300);

        static double spinning = 0;
        static bool spinningLastFrame = false;
        public static void Update()
        {
            spinning -= MainWindow.deltaTime;
        }
        public static void Render()
        {
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH),10,new Vector3(0,0,0), new Vector3(2, 114, 171));//pozadi
            Draw.RenderImage("Assets/Automaty/slotMachine.png", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH));

            if (spinning > 0)
            {
                spinningLastFrame = true;
                //toceni

            }
            else if (spinningLastFrame)
            {
                spinningLastFrame = false;
                //vysledek toceni



            }



            if (MainWindow.toralFrames % 2 == 0) Draw.RenderLabel("SPIN", SPINpos,SPINsize, 50f, new Vector3(0, 0, 0), new Vector3(255, 255, 230));
            else Draw.RenderLabel("SPIN", SPINpos, SPINsize, 50f, new Vector3(0, 0, 0), new Vector3(230, 255, 255));
        }
        public static void LeftClick(Vector2 clickPos)
        {
            if(SF.DidIClick(clickPos, SPINpos, SPINsize))
            {
                spinning = 5;
            }
        }
    }
}
