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
        static Random rng = new Random();

        private static Vector2 iconSize = new Vector2(400, 400);
        private static Vector2[] slotpos = { new Vector2(SF.SW / 4, SF.SH / 2), new Vector2(SF.SW * 2 / 4, SF.SH / 2) , new Vector2(SF.SW * 3 / 4, SF.SH / 2) };
        private static string[] slotdrop = {"7","7","7" };

        static Vector2 SPINpos = new Vector2(SF.SW-200, SF.SH/2);
        static Vector2 SPINsize = new Vector2(300, 300);

        static double spinning = 0;
        static double delkaSpinu = 1; // v sekundach
        static bool spinningLastFrame = false; // je vyuzito pro zjisteni Last Frame
        private static int costToPlay = 100;
        public static int CostToPlay { get => costToPlay; }

        public static void Update()
        {
            spinning -= MainWindow.deltaTime;
            if (spinning > 0)
            {
                spinningLastFrame = true;
                //toceni

                for (int i = 0; i < 3; i++)
                {
                    switch (rng.Next(0, 5)) // 0 1 2 3 4
                    {
                        case 0:
                            slotdrop[i] = "7";
                            break;
                        case 1:
                            slotdrop[i] = "diamond";
                            break;
                        case 2:
                            slotdrop[i] = "berry";
                            break;
                        case 3:
                            slotdrop[i] = "apple";
                            break;
                        case 4:
                            slotdrop[i] = "lemon";
                            break;
                    }
                }

            }
            else if (spinningLastFrame)
            {
                spinningLastFrame = false;
                //vysledek toceni


                if(slotdrop[0] == slotdrop[1] && slotdrop[1] == slotdrop[2])    //3x
                {
                    switch(slotdrop[0])
                    {
                        case "7": 
                            MainWindow.Money += CostToPlay * 10;
                            break;
                        case "diamond":
                            MainWindow.Money += CostToPlay * 5;
                            break;
                        case "berry":
                            MainWindow.Money += CostToPlay * 3;
                            break;
                        case "apple":
                            MainWindow.Money += CostToPlay * 3;
                            break;
                        case "lemon":
                            MainWindow.Money += CostToPlay * 3;
                            break;
                    }
                }
                else if(slotdrop[0] == slotdrop[1] || slotdrop[1] == slotdrop[2] || slotdrop[2] == slotdrop[0])   //2x
                {
                    string doubledItemName;
                    if(slotdrop[0] == slotdrop[1]) doubledItemName = slotdrop[0];
                    else doubledItemName = slotdrop[2];

                    switch (doubledItemName)
                    {
                        case "7": 
                            MainWindow.Money += CostToPlay * 3;
                            break;
                        case "diamond":
                            MainWindow.Money += CostToPlay * 2;
                            break;
                        case "berry":
                            MainWindow.Money += CostToPlay * 1;
                            break;
                        case "apple":
                            MainWindow.Money += CostToPlay * 1;
                            break;
                        case "lemon":
                            MainWindow.Money += CostToPlay * 1;
                            break;
                    }
                }
                if(slotdrop[0] == "berry") MainWindow.Money += (int)(CostToPlay * 0.1);
            }

        }
        public static void Render()
        {
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH),10,new Vector3(0,0,0), new Vector3(2, 114, 171));//pozadi
            Draw.RenderImage("Assets/Automaty/slotMachine.png", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH));

            //nakresli ovoce 
            for(int i = 0; i < 3; i++) Draw.RenderImage("Assets/Automaty/" + slotdrop[i] + ".png", slotpos[i], iconSize);

            if (MainWindow.toralFrames % 2 == 0) Draw.RenderLabel("SPIN", SPINpos,SPINsize, 50f, new Vector3(0, 0, 0), new Vector3(255, 255, 230));
            else Draw.RenderLabel("SPIN", SPINpos, SPINsize, 50f, new Vector3(0, 0, 0), new Vector3(230, 255, 255));

            Vector2 costPos = new Vector2(SF.SW / 2, SF.SH / 6);
            Vector2 costSize = new Vector2(800, 200);
            Draw.RenderLabel(CostToPlay + "$ to play", costPos, costSize, 80, new Vector3(0, 0, 0), new Vector3(255,255,255));
        }
        public static void LeftClick(Vector2 clickPos)
        {

            if(SF.DidIClick(clickPos, SPINpos, SPINsize))
            {            
                if (spinning > -0.15f) return;

                spinning = delkaSpinu;
                MainWindow.Money -= CostToPlay;
            }
        }
    }
}
