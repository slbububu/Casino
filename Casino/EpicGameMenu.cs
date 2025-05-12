using Casino.Baseball;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class EpicGameMenu
    {
        private static Vector2 iconSize = new Vector2(300, 200);
        private static Vector3 BackgrounCol = new Vector3(255, 254, 222);
        private static int fontSize = 100;

        private static float smallMultNum = 1.1f;
        private static float mediumMultNum = 1.5f;
        private static float bigMultNum = 2f;
        private static float massiveMultNum = 5f;
        private static Vector2 smallMult = new Vector2(SF.SW / 5, SF.SH / 2);
        private static Vector2 mediumMult = new Vector2(SF.SW * 2 / 5, SF.SH / 2);
        private static Vector2 bigMult = new Vector2(SF.SW * 3 / 5, SF.SH / 2);
        private static Vector2 massiveMult = new Vector2(SF.SW * 4 / 5, SF.SH / 2);

        private static int costToPlay = 100;
        public static int CostToPlay { get => costToPlay; }

        public static void Update()
        {

        }
        public static void Render()
        {
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH), 10, new Vector3(0, 0, 0), new Vector3(2, 114, 171));//pozadi

            Draw.RenderLabel(smallMultNum + "X", smallMult, iconSize, fontSize, new Vector3(0, 255, 0), BackgrounCol);
            Draw.RenderLabel(mediumMultNum + "X", mediumMult, iconSize, fontSize, new Vector3(255, 200, 0), BackgrounCol);
            Draw.RenderLabel(bigMultNum + "X", bigMult, iconSize, fontSize, new Vector3(255, 100, 0), BackgrounCol);
            Draw.RenderLabel(massiveMultNum + "X", massiveMult, iconSize, fontSize, new Vector3(255, 0, 100), BackgrounCol);
        
            Vector2 costPos = new Vector2(SF.SW / 2, SF.SH / 4);
            Vector2 costSize = new Vector2(800,200);
            Draw.RenderLabel(CostToPlay + "$ to play", costPos, costSize, 80, new Vector3(0, 0, 0), BackgrounCol);
        }
        public static void LeftClick(Vector2 clickPos)
        {
            if (SF.DidIClick(clickPos, smallMult, iconSize))
            {
                MainWindow.selectedHra = "baseball";
                BaseballMain.SetDifficulty(0.8f, 0, smallMultNum);
            }
            if (SF.DidIClick(clickPos, mediumMult, iconSize))
            {
                MainWindow.selectedHra = "baseball";
                BaseballMain.SetDifficulty(0.7f, 1, mediumMultNum);
            }
            if (SF.DidIClick(clickPos, bigMult, iconSize))
            {
                MainWindow.selectedHra = "baseball";
                BaseballMain.SetDifficulty(0.6f, 2, bigMultNum);
            }
            if (SF.DidIClick(clickPos, massiveMult, iconSize))
            {
                MainWindow.selectedHra = "baseball";
                BaseballMain.SetDifficulty(0.5f, 3, massiveMultNum);
            }

            if (SF.DidIClick(clickPos, smallMult, iconSize) || SF.DidIClick(clickPos, mediumMult, iconSize) || SF.DidIClick(clickPos, bigMult, iconSize) || SF.DidIClick(clickPos, massiveMult, iconSize))
            {
                MainWindow.Money -= CostToPlay;
            }
        }
    }
}
