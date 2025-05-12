using Casino;
using Casino.Baseball;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EpicGame
{
    public static class GameManager
    {
        public static Oponent[] oponent = {
            new Oponent(200, 35, new Vector2(1, 1), true, true,"tunSahur",new Vector2(0.8f,0.4f),new Vector2(0,0)),
            new Oponent(200, 50, new Vector2(0.5f, 0.5f), false, false,"downSyndrom",new Vector2(0.5f,1),new Vector2(0.5f,0)),
            new Oponent(200, 90, new Vector2(2, 1), true, true,"funnyGuy",new Vector2(1,0.7f),new Vector2(0,0.3f)),
            new Oponent(200, 110, new Vector2(2f, 0.5f), true, false,"terrorist",new Vector2(0.9f,0.9f),new Vector2(0,0))};

        static float throwTime = 0.7f; //TODO TOTO JE VE JMENU TE FCE
        static short oponentCurent = 0; //TODO TOTO JE VE JMENU TE FCE
        static public int state = 1;
        static Timer tim = new Timer();
        static float waitTime = 0.5f;

        public static void Initialize(float throwTime, short oponentCurent)
        {
            GameManager.throwTime = throwTime;
            GameManager.oponentCurent = oponentCurent;        
        }
        public static void Update()
        {
            switch (state)
            {
                case 1:
                    Cam.LookatRotation(Cam.defaultRotation);
                    Draw.RenderImage("Baseball/Enemies/tunSahur.png", new Vector2(500, 500), new Vector2(500, 500));

                    if (tim.Wait(1))
                    {
                        state = 2;
                        Pich.Throw(throwTime, GetOponent());
                    }
                    break;
                case 2:
                    if (Pich.ballState == "misnuty")
                    {
                        state = 4;
                    }
                    if (Pich.ballState == "odpalen")
                    {
                        state = 3;
                    }
                    break;
                case 3:
                    Cam.Lookat(Pich.pos);

                    if (Pich.ballState == "nazemi")
                    {
                        state = 4;
                    }
                    if (Pich.ballState == "win")
                    {
                        state = 5;
                        Draw.Zajasat(1);
                    }
                    break;
                case 4:
                    if (tim.Wait(1))
                    {
                        //LOSE 

                        MainWindow.selectedHra = "epicGameMenu";
                        state = 1;
                    }
                    break;
                case 5:
                    if (tim.Wait(1))
                    {
                        //WIN

                        MainWindow.Money += BaseballMain.reward;

                        MainWindow.selectedHra = "epicGameMenu";
                        state = 1;
                    }
                    break;
            }
        }
        public static Oponent GetOponent()
        {
            if (oponentCurent >= oponent.Length) return new Oponent(100, 1, new Vector2(100, 100), true, true, "neexistujici", new Vector2(0, 0), new Vector2(0, 0));
            return oponent[oponentCurent];
        }
    }

    public class Oponent(float fenceDistance, uint pocetDivaku, Vector2 expDiff, bool exponentalX, bool exponentalY, string fileName, Vector2 OblibeneMult, Vector2 OblibeneOftset)
    {
        //todo png hrace
        public float fenceDistance = fenceDistance;
        public uint pocetDivaku = pocetDivaku;
        public Vector2 expDiff = expDiff;
        public bool exponentalX = exponentalX;
        public bool exponentalY = exponentalY;
        public string fileName = fileName;

        public Vector2 OblibeneMult = OblibeneMult;
        public Vector2 OblibeneOftset = OblibeneOftset;
    }
    internal class Timer
    {
        double start;
        double lenght;
        bool settable = true;
        public void Set(double lenght)
        {
            if (!settable) return;

            settable = false;
            start = MainWindow.toralTime;
            this.lenght = lenght;
        }
        public bool Get()
        {
            if (start + lenght < MainWindow.toralTime)
            {
                settable = true;
                return true;
            }
            return false;
        }
        public bool Wait(double lenght)
        {
            if (settable)
            {
                settable = false;
                start = MainWindow.toralTime;
                this.lenght = lenght;
            }

            if (start + lenght < MainWindow.toralTime)
            {
                settable = true;
                return true;
            }
            return false;
        }
    }
}