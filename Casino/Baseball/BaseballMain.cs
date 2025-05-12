using EpicGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Casino.Baseball
{
    public static class BaseballMain
    {
        public static bool playerHasControl = false;//debug
        public static bool playerHasControlChanged = false;//debug
        public static int canvasHeight = 1080;
        public static int canvasWidth = 1920;
        public static int reward = 1;
        public static void Update()
        {
            GameManager.Update();
            Cam.Update();
            Pich.Update();
            MainWindow.canEscape = false;
        }
        public static void Render()
        {
            Draw.Background(Brushes.SkyBlue);
            Draw.Ground(Brushes.Green);
            if (Pich.isBallBehindFence) Pich.RenderBall();
            Draw.Divaci(GameManager.GetOponent().fenceDistance + 10, GameManager.GetOponent().pocetDivaku);
            Draw.Fence(GameManager.GetOponent().fenceDistance, Brushes.Yellow);
            if (!Pich.isBallBehindFence && !Pich.IsBallFartherThat(Pich.StartPos.X)) Pich.RenderBall();
            Draw.Enemy();
            if (!Pich.isBallBehindFence && Pich.IsBallFartherThat(Pich.StartPos.X)) Pich.RenderBall();
            Pich.RenderAimRectangle();
            Pich.Renderbat();
        }
        public static void LeftClick(Vector2 clickPos)
        {
            Pich.Swing(clickPos);
        }
        public static void SetDifficulty(float throwTime, short oponentCurent,float rewardMult)
        {
            GameManager.Initialize(throwTime, oponentCurent);
            reward = (int)(rewardMult * EpicGameMenu.CostToPlay);
        }
    }
}
