using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class Ruleta
    {
        static Random rng = new Random();

        static int selectedNum = -1;
        static int RNGNum = -1;
        static double spinning = -99;
        static double delkaSpinu = 1; // v sekundach
        static bool spinningLastFrame = false; // je vyuzito pro zjisteni Last Frame
        static double breakPosSpinu = 0.65;
        static int costToPlay = 100;
        public static int CostToPlay { get => costToPlay; }

        static Vector2 POLESIZE = new Vector2(100, 75);
        static Vector2 POLEODSTUP = new Vector2(2, 2);
        static Vector2 POLEODOFSET = new Vector2(700, SF.SH/2 - POLESIZE.Y - POLEODSTUP.Y);
        static int FONTSIZE = 50;
        static int pocetPolicek = 37;
        static Vector2[] policka = new Vector2[pocetPolicek];
        static Vector2 ruletaPos = new Vector2(400, SF.SH / 2);
        static Vector2 BlackPos = new Vector2(SF.SW/2 - 300, 200);
        static Vector2 RedPos = new Vector2(SF.SW/2 + 300, 200);
        static Vector2 ColourSize = new Vector2(250,150);
        public static void Setup()
        {
            policka[0] = new Vector2(-1 * (POLESIZE.X + POLEODSTUP.X),1 * (POLESIZE.Y + POLEODSTUP.Y));

            for (int x = 0; x < 12; x++)
            for (int y = 0; y < 3; y++)
            {
                    int iPole = x * 3 + y + 1; //index Pole

                    policka[iPole] = new Vector2(
                        x * (POLESIZE.X + POLEODSTUP.X),
                        y * (POLESIZE.Y + POLEODSTUP.Y));
            }

            for (int i = 0; i < pocetPolicek; i++) policka[i] += POLEODOFSET;
        }
        public static void Update()
        {
            if (spinning >= 0)
            {
                spinningLastFrame = true;
                //toceni

                //animace (animace je v tomto pripade primo v renderu)
            }
            else if (spinningLastFrame)
            {
                spinningLastFrame = false;
                //vysledek toceni

                //reward
                RNGNum = rng.Next(0, pocetPolicek);
                //RNGNum = rng.Next(0, 2); //debug

                if (selectedNum == RNGNum) MainWindow.Money += (pocetPolicek - 1) * CostToPlay;
                else if (selectedNum >= 100) //  RED / BLACK
                {
                    if(selectedNum != 0)
                    {
                        if (selectedNum % 2 == RNGNum) MainWindow.Money += 2 * CostToPlay;
                    }
                }

                selectedNum = -1; //hracova volba reset
            }
            spinning -= MainWindow.deltaTime;
        }
        public static void Render()
        {
            //pozadi
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH), 10, new Vector3(0, 0, 0), new Vector3(2, 114, 171));//pozadi
            
            //cisla
            for (int i = 0; i < pocetPolicek; i++)
            {
                Vector3 BGcolor = new Vector3(255, 0, 0); //RED
                if(i % 2 == 0) BGcolor = new Vector3(0, 0, 0); //BLACK
                if(i == 0) BGcolor = new Vector3(0, 255, 0); //GREEN

                Vector3 textcolor = new Vector3(255, 255, 255);
                if(i == selectedNum) textcolor = new Vector3(245, 178, 78);

                Draw.RenderLabel("" + i, policka[i], POLESIZE, FONTSIZE, textcolor, BGcolor);//pozadi
            }

            //ruleta
            if (spinning > 0) Draw.RenderImage("Assets/Ruleta/ruleta.png", ruletaPos, new Vector2(300, 300), (float)MainWindow.toralTime * 2000);
            else Draw.RenderImage("Assets/Ruleta/ruleta.png", ruletaPos, new Vector2(300, 300), 0);

            if(0 > spinning && spinning > -breakPosSpinu)
            {
                Vector3 BGcolor = new Vector3(255, 0, 0); //RED
                if (RNGNum % 2 == 0) BGcolor = new Vector3(0, 0, 0); //BLACK
                if (RNGNum == 0) BGcolor = new Vector3(0, 255, 0); //GREEN

                Vector3 textcolor = new Vector3(255, 255, 255);

                Vector2 losovaneCisloPos = ruletaPos + new Vector2(0,250);

                Draw.RenderLabel("" + RNGNum, losovaneCisloPos, POLESIZE * 2, FONTSIZE * 2, textcolor, BGcolor);//pozadi
            }

            //  RED / BLACK
            Draw.RenderLabel("BLACK", BlackPos, ColourSize, 100, new Vector3(255, 255, 255), new Vector3(0, 0, 0));
            Draw.RenderLabel("RED", RedPos, ColourSize, 100, new Vector3(255, 255, 255), new Vector3(255, 0, 0));



            //debug
            //Draw.RenderLabel("" + selectedNum, new Vector2(100, 100), new Vector2(100, 100), 50, new Vector3(0, 0, 0), new Vector3(2, 114, 171));//pozadi
        }
        public static void LeftClick(Vector2 clickPos)
        {
            //cisla
            for (int i = 0; i < pocetPolicek; i++)
            {
                if(SF.DidIClick(clickPos, policka[i], POLESIZE))
                {            
                    if (spinning > -breakPosSpinu) return;

                    spinning = delkaSpinu;
                    spinningLastFrame = true;
                    MainWindow.Money -= CostToPlay;

                    selectedNum = i;
                }
            }

            //      RED / BLACK
            if (SF.DidIClick(clickPos, BlackPos, ColourSize))
            {
                if (spinning > -breakPosSpinu) return;

                spinning = delkaSpinu;
                spinningLastFrame = true;
                MainWindow.Money -= CostToPlay;

                selectedNum = 100;
            }
            if (SF.DidIClick(clickPos, RedPos, ColourSize))
            {
                if (spinning > -breakPosSpinu) return;

                spinning = delkaSpinu;
                spinningLastFrame = true;
                MainWindow.Money -= CostToPlay;

                selectedNum = 101;
            }
        }
    }
}
